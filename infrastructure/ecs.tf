# Get latest image in provided ECR
data "aws_ecr_image" "service_image" {
  repository_name = var.docker_repository
  most_recent     = true
}

resource "aws_ecs_cluster" "betterreads_cluster" {
  name = "betterreads-cluster"
}

resource "aws_ecs_task_definition" "ecs_task_definition" {
  family             = "my-ecs-task"
  network_mode       = "awsvpc"
  execution_role_arn = var.ecs_role_arn
  cpu                = 256
  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture        = "X86_64"
  }
  container_definitions = jsonencode([
    {
      name      = "dockergs"
      image     = data.aws_ecr_image.service_image.image_uri,
      cpu       = 256
      memory    = 512
      essential = true
      portMappings = [
        {
          containerPort = 8080
          hostPort      = 8080
          protocol      = "tcp"
        }
      ]
    }
  ])
}

resource "aws_ecs_capacity_provider" "ecs_capacity_provider" {
  name = "betterreads_capacity_provider"

  auto_scaling_group_provider {
    auto_scaling_group_arn = aws_autoscaling_group.ecs_asg.arn

    managed_scaling {
      maximum_scaling_step_size = 1000
      minimum_scaling_step_size = 1
      status                    = "ENABLED"
      target_capacity           = 3
    }
  }
}

resource "aws_ecs_cluster_capacity_providers" "cluster_cp" {
  cluster_name       = aws_ecs_cluster.betterreads_cluster.name
  capacity_providers = [aws_ecs_capacity_provider.ecs_capacity_provider.name]

  default_capacity_provider_strategy {
    base              = 1
    weight            = 100
    capacity_provider = aws_ecs_capacity_provider.ecs_capacity_provider.name
  }
}

# Output service image name for debugging
output "docker-image" {
  value = data.aws_ecr_image.service_image.image_uri
}