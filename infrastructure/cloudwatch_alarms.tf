# EC2 Alarms
resource "aws_cloudwatch_metric_alarm" "high_cpu" {
  alarm_name          = "high-cpu-alarm"
  comparison_operator = "GreaterThanOrEqualToThreshold"
  evaluation_periods  = "2"
  metric_name         = "CPUUtilization"
  namespace           = "AWS/EC2"
  period              = "60"
  statistic           = "Average"
  threshold           = "70"

  alarm_description = "This alarm monitors high CPU utilization"
  dimensions = {
    AutoScalingGroupName = aws_autoscaling_group.ecs_asg.name
  }

  actions_enabled = true
}


resource "aws_cloudwatch_log_group" "asg_log_group" {
  name              = "/asg/your-instance-logs"
  retention_in_days = 14
}


# ECS Alarms
resource "aws_cloudwatch_metric_alarm" "ecs_cpu_high" {
  alarm_name          = "ecs-high-cpu"
  comparison_operator = "GreaterThanOrEqualToThreshold"
  evaluation_periods  = "2"
  metric_name         = "CPUUtilization"
  namespace           = "AWS/ECS"
  period              = "60"
  statistic           = "Average"
  threshold           = "80"

  alarm_description = "Alarm when ECS Task CPU exceeds 80%"
  dimensions = {
    ClusterName = aws_ecs_cluster.betterreads_cluster.name
    ServiceName = aws_ecs_service.ecs_service.name
  }
}

resource "aws_cloudwatch_log_group" "ecs_log_group" {
  name              = "/ecs/your-cluster-logs"
  retention_in_days = 14
}

# ALB Alarms
resource "aws_cloudwatch_metric_alarm" "alb_5xx_errors" {
  alarm_name          = "alb-5xx-errors"
  comparison_operator = "GreaterThanOrEqualToThreshold"
  evaluation_periods  = "2"
  metric_name         = "HTTPCode_ELB_5XX_Count"
  namespace           = "AWS/ApplicationELB"
  period              = "60"
  statistic           = "Sum"
  threshold           = "5"

  alarm_description = "Alarm when ALB 5xx errors exceed 5"
  dimensions = {
    LoadBalancer = aws_lb.ecs_alb.name
  }
}

resource "aws_cloudwatch_log_group" "alb_log_group" {
  name              = "/alb/your-load-balancer-logs"
  retention_in_days = 14
}
