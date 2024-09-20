# VPC
resource "aws_vpc" "main" {
  cidr_block           = var.vpc_cidr
  enable_dns_hostnames = true
  enable_dns_support   = true
  tags = {
    name = "main"
  }
}

# Internet Gateway
resource "aws_internet_gateway" "internet_gateway" {
  vpc_id = aws_vpc.main.id
  tags = {
    Name = "internet_gateway"
  }
}

# NAT Gateway Elastic IP
resource "aws_eip" "nat_eip" {
  domain                    = "vpc"
  associate_with_private_ip = "10.0.0.5"
  depends_on                = [aws_internet_gateway.internet_gateway]
}

# NAT Gateway in the public subnet
resource "aws_nat_gateway" "nat_gateway" {
  allocation_id = aws_eip.nat_eip.id
  subnet_id     = aws_subnet.public-subnet-1.id
  depends_on    = [aws_eip.nat_eip]
}

# Public subnets
resource "aws_subnet" "public-subnet-1" {
  tags = {
    Name = "public-terraform-subnet-1"
  }
  cidr_block        = "10.0.1.0/24"
  vpc_id            = aws_vpc.main.id
  availability_zone = var.subnet_region_a
}
resource "aws_subnet" "public-subnet-2" {
  tags = {
    Name = "public-terraform-subnet-2"
  }
  cidr_block        = "10.0.2.0/24"
  vpc_id            = aws_vpc.main.id
  availability_zone = var.subnet_region_b
}

# Private subnets
resource "aws_subnet" "private-subnet-1" {
  tags = {
    Name = "private-terraform-subnet-1"
  }
  cidr_block        = "10.0.3.0/24"
  vpc_id            = aws_vpc.main.id
  availability_zone = var.subnet_region_a
}
resource "aws_subnet" "private-subnet-2" {
  tags = {
    Name = "private-terraform-subnet-2"
  }
  cidr_block        = "10.0.4.0/24"
  vpc_id            = aws_vpc.main.id
  availability_zone = var.subnet_region_b
}

# Route tables for the subnets
resource "aws_route_table" "public-route-table" {
  vpc_id = aws_vpc.main.id
  tags = {
    Name = "public-terraform-route-table"
  }
}
resource "aws_route_table" "private-route-table" {
  vpc_id = aws_vpc.main.id
  tags = {
    Name = "private-terraform-route-table"
  }
}

# Route the public subnet traffic through the Internet Gateway
resource "aws_route" "public-internet-igw-route" {
  route_table_id         = aws_route_table.public-route-table.id
  gateway_id             = aws_internet_gateway.internet_gateway.id
  destination_cidr_block = "0.0.0.0/0"
}

# Route NAT Gateway
resource "aws_route" "nat-ngw-route" {
  route_table_id         = aws_route_table.private-route-table.id
  nat_gateway_id         = aws_nat_gateway.nat_gateway.id
  destination_cidr_block = "0.0.0.0/0"
}

# Associate the newly created route tables to the subnets
resource "aws_route_table_association" "public-route-1-association" {
  route_table_id = aws_route_table.public-route-table.id
  subnet_id      = aws_subnet.public-subnet-1.id
}
resource "aws_route_table_association" "public-route-2-association" {
  route_table_id = aws_route_table.public-route-table.id
  subnet_id      = aws_subnet.public-subnet-2.id
}
resource "aws_route_table_association" "private-route-1-association" {
  route_table_id = aws_route_table.private-route-table.id
  subnet_id      = aws_subnet.private-subnet-1.id
}
resource "aws_route_table_association" "private-route-2-association" {
  route_table_id = aws_route_table.private-route-table.id
  subnet_id      = aws_subnet.private-subnet-2.id
}