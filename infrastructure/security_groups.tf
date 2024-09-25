# Security Group for ECS Nodes
resource "aws_security_group" "ec2_security_group" {
  name   = "ec2_security_group"
  vpc_id = aws_vpc.main.id

  ingress {
    from_port       = 0
    to_port         = 0
    protocol        = "-1"
    security_groups = [aws_security_group.elb_security_group.id]
  }

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 8080
    to_port     = 8080
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# Security Group for Load Balancer
resource "aws_security_group" "elb_security_group" {
  name        = "load_balancer_security_group"
  description = "Controls access to the ALB"
  vpc_id      = aws_vpc.main.id

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 8080
    to_port     = 8080
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# resource "aws_ec2_instance_connect_endpoint" "ec2_connect_endpoint_1" {
#   count = var.debug ? 1 : 0
#   subnet_id = aws_subnet.private-subnet-1.id
#   security_group_ids = [ aws_security_group.ec2_security_group.id ]
#   tags = {
#     Name = "subnet-1-endpoint"
#   }
# }

# resource "aws_ec2_instance_connect_endpoint" "ec2_connect_endpoint_2" {
#   count = var.debug ? 1 : 0
#   subnet_id = aws_subnet.private-subnet-2.id
#   security_group_ids = [ aws_security_group.ec2_security_group.id ]
#   tags = {
#     Name = "subnet-2-endpoint"
#   }
# }