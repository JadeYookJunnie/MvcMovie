resource "aws_cloudwatch_dashboard" "service_dashboard" {
  dashboard_name = "ServiceMonitoringDashboard"

  dashboard_body = jsonencode({
    widgets = [
      {
        "type" : "metric",
        "x" : 0,
        "y" : 0,
        "width" : 12,
        "height" : 6,
        "properties" : {
          "metrics" : [
            ["AWS/ApplicationELB", "RequestCount", "LoadBalancer", "${aws_lb.ecs_alb.arn_suffix}"],
            ["AWS/ApplicationELB", "HTTPCode_Target_2XX_Count", "LoadBalancer", "${aws_lb.ecs_alb.arn_suffix}"],
            ["AWS/ApplicationELB", "HTTPCode_Target_4XX_Count", "LoadBalancer", "${aws_lb.ecs_alb.arn_suffix}"],
            ["AWS/ApplicationELB", "HTTPCode_Target_5XX_Count", "LoadBalancer", "${aws_lb.ecs_alb.arn_suffix}"]
          ],
          "view" : "timeSeries",
          "stacked" : false,
          "region" : "ap-southeast-2",
          "title" : "ALB - Request Count and HTTP Codes",
          "period" : 300
        }
      },
      {
        "type" : "metric",
        "x" : 12,
        "y" : 0,
        "width" : 12,
        "height" : 6,
        "properties" : {
          "metrics" : [
            ["AWS/ECS", "CPUUtilization", "ClusterName", "${aws_ecs_cluster.betterreads_cluster.name}", "ServiceName", "my-ecs-service"],
            ["AWS/ECS", "MemoryUtilization", "ClusterName", "${aws_ecs_cluster.betterreads_cluster.name}", "ServiceName", "my-ecs-service"]
          ],
          "view" : "timeSeries",
          "stacked" : false,
          "region" : "ap-southeast-2",
          "title" : "ECS - CPU and Memory Utilization",
          "period" : 300
        }
      },
      {
        "type" : "metric",
        "x" : 0,
        "y" : 6,
        "width" : 12,
        "height" : 6,
        "properties" : {
          "metrics" : [
            ["AWS/EC2", "CPUUtilization", "AutoScalingGroupName", "${aws_autoscaling_group.ecs_asg.name}"],
            ["AWS/EC2", "NetworkIn", "AutoScalingGroupName", "${aws_autoscaling_group.ecs_asg.name}"],
            ["AWS/EC2", "NetworkOut", "AutoScalingGroupName", "${aws_autoscaling_group.ecs_asg.name}"]
          ],
          "view" : "timeSeries",
          "stacked" : false,
          "region" : "ap-southeast-2",
          "title" : "ASG - CPU and Network",
          "period" : 300
        }
      },
      {
        "type" : "metric",
        "x" : 12,
        "y" : 6,
        "width" : 12,
        "height" : 6,
        "properties" : {
          "metrics" : [
            ["AWS/ECS", "RunningTasksCount", "ClusterName", "${aws_ecs_cluster.betterreads_cluster.name}"],
            ["AWS/ECS", "PendingTasksCount", "ClusterName", "${aws_ecs_cluster.betterreads_cluster.name}"]
          ],
          "view" : "timeSeries",
          "stacked" : false,
          "region" : "ap-southeast-2",
          "title" : "ECS - Task Count",
          "period" : 300
        }
      },
      {
        "type" : "metric",
        "x" : 0,
        "y" : 12,
        "width" : 24,
        "height" : 6,
        "properties" : {
          "metrics" : [
            ["AWS/ApplicationELB", "HealthyHostCount", "TargetGroup", "${aws_lb_target_group.ecs_tg.arn_suffix}"],
            ["AWS/ApplicationELB", "UnHealthyHostCount", "TargetGroup", "${aws_lb_target_group.ecs_tg.arn_suffix}"]
          ],
          "view" : "timeSeries",
          "stacked" : false,
          "region" : "ap-southeast-2",
          "title" : "ALB - Target Health",
          "period" : 300
        }
      }
    ]
  })
}
