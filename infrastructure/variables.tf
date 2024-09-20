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

variable "docker_repository" {
  description = "Name for ECR"
  type        = string
  default     = "terraform-docker-repo"
}

variable "debug" {
  description = "Enables creation of debug objects (Such as VPC Endpoint)"
  type        = bool
  default     = false
}