resource "aws_default_vpc" "default_vpc" {
}

resource "aws_default_subnet" "default_subnet_a" {
  availability_zone = var.subnet_region_a
}

resource "aws_default_subnet" "default_subnet_b" {
  availability_zone = var.subnet_region_b
}
