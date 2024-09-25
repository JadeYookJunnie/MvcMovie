# Get ECS compatible image
data "aws_ssm_parameter" "ecs_node_ami" {
  name = "/aws/service/ecs/optimized-ami/amazon-linux-2/recommended/image_id"
}

# Create EC2 Role
resource "aws_iam_role" "ec2_arn" {
  name = "AmazonEC2ContainerServiceforEC2Role"
  assume_role_policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [
      {
        "Action" : "sts:AssumeRole",
        "Effect" : "Allow",
        "Principal" : {
          "Service" : "ec2.amazonaws.com"
        }
      }
    ]
  })
}

# Attach EC2 Role
resource "aws_iam_role_policy_attachment" "ecs_task_execution_role_policy" {
  role       = aws_iam_role.ec2_arn.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonEC2ContainerServiceforEC2Role"
}

# Attach Instance Connect Role
# resource "aws_iam_role_policy_attachment" "ec2_instance_connect" {
#   count = var.debug ? 1 : 0
#   role       = aws_iam_role.ec2_arn.name
#   policy_arn = "arn:aws:iam::aws:policy/EC2InstanceConnect"
# }

# Attach Session Manager Role
resource "aws_iam_role_policy_attachment" "ec2_session_manager_role" {
  count = var.debug ? 1 : 0
  role       = aws_iam_role.ec2_arn.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonSSMManagedInstanceCore"
}

# Create an instance profile
resource "aws_iam_instance_profile" "ecs_instance_profile" {
  name = "ecsInstanceProfile"
  role = aws_iam_role.ec2_arn.name
}

# EC2 Launch Template for ASG
resource "aws_launch_template" "ecs_lt" {
  name_prefix            = "ecs-template"
  image_id               = data.aws_ssm_parameter.ecs_node_ami.value
  instance_type          = "t2.micro"
  vpc_security_group_ids = [aws_security_group.ec2_security_group.id]

  iam_instance_profile {
    arn = aws_iam_instance_profile.ecs_instance_profile.arn
  }

  block_device_mappings {
    device_name = "/dev/xvda"
    ebs {
      volume_size = 30
      volume_type = "gp2"
    }
  }

  tag_specifications {
    resource_type = "instance"
    tags = {
      Name = "ecs-instance"
    }
  }

  user_data = base64encode(<<-EOF
      #!/bin/bash
      echo ECS_CLUSTER=${aws_ecs_cluster.betterreads_cluster.name} >> /etc/ecs/ecs.config;
    EOF
  )

  depends_on = [aws_iam_instance_profile.ecs_instance_profile]
}

# Auto-Scaling Group
resource "aws_autoscaling_group" "ecs_asg" {
  vpc_zone_identifier = [aws_subnet.private-subnet-1.id, aws_subnet.private-subnet-2.id]
  desired_capacity    = 1
  max_size            = 1
  min_size            = 1

  launch_template {
    id      = aws_launch_template.ecs_lt.id
    version = "$Latest"
  }
  tag {
    key                 = "AutoScalingGroupName"
    value               = "BetterReads-ASG"
    propagate_at_launch = true
  }
  tag {
    key                 = "AmazonECSManaged"
    value               = true
    propagate_at_launch = true
  }

  depends_on = [aws_launch_template.ecs_lt]
}
