# Application Load Balancer
resource "aws_lb" "ecs_alb" {
  name                       = "ecs-alb"
  internal                   = false
  load_balancer_type         = "application"
  security_groups            = [aws_security_group.elb_security_group.id]
  subnets                    = [aws_subnet.public-subnet-1.id, aws_subnet.public-subnet-2.id]
  enable_deletion_protection = false
  idle_timeout = 30

  tags = {
    Name = "ecs-alb"
  }
}

# Load Balancer Listener
resource "aws_lb_listener" "ecs_alb_listener" {
  load_balancer_arn = aws_lb.ecs_alb.arn
  port              = 80
  protocol          = "HTTP"
  depends_on        = [aws_lb_target_group.ecs_tg]

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.ecs_tg.arn
  }
}

# ECS Target Group
resource "aws_lb_target_group" "ecs_tg" {
  name        = "ecs-target-group"
  port        = 8080
  protocol    = "HTTP"
  target_type = "instance"
  vpc_id      = aws_vpc.main.id

  health_check {
    path                = "/"
    port                = "traffic-port"
    healthy_threshold   = 5
    unhealthy_threshold = 2
    timeout             = 2
    interval            = 60
    matcher             = "200"
  }
}