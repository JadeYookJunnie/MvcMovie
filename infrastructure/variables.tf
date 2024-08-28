variable "region" {
  description = "The AWS region to deploy the resources"
  type        = string
  default     = "ap-southeast-2"
}

variable "subnet_region_a" {
  description = "The AWS region for subnet a"
  type        = string
  default     = "ap-southeast-2a"
}

variable "subnet_region_b" {
  description = "The AWS region for subnet b"
  type        = string
  default     = "ap-southeast-2b"
}

variable "profile" {
  description = "The IAM user to launch instances"
  type        = string
  default     = "Terraform"
}

variable "vpc_cidr" {
  description = "VPC CIDR Range"
  type        = string
  default     = "10.0.0.0/16"
}

variable "ecs_role_arn" {
  description = "ARN for ECS Cluster. (Minimun ecsTaskExecutionRole)"
  type = string
}

variable "iam_instance_arn" {
  description = "ARN for EC2 Template. (Minimun ecsInstanceProfile)"
  type = string
}

variable "docker_repository_url" {
  description = "Url for ECR"
  type = string
  default = "851725582693.dkr.ecr.ap-southeast-2.amazonaws.com/terraform-docker-repo:latest"
}