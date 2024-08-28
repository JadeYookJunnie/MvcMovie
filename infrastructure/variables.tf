variable "region" {
  description = "The AWS region to deploy the resources"
  type        = string
  default     = "ap-southeast-2"
}

variable "subnet_region_a" {
  description = "The AWS region to deploy the resources"
  type        = string
  default     = "ap-southeast-2a"
}

variable "subnet_region_b" {
  description = "The AWS region to deploy the resources"
  type        = string
  default     = "ap-southeast-2b"
}

variable "profile" {
  description = "The IAM user to launch instances"
  type        = string
  default     = "Terraform"
}