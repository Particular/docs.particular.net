docker pull particular/servicecontrol.azureservicebus.init-windows:4
docker pull particular/servicecontrol.azureservicebus-windows:4
docker pull particular/servicecontrol.azureservicebus.monitoring.init-windows:4
docker pull particular/servicecontrol.azureservicebus.monitoring-windows:4
docker pull particular/servicecontrol.azureservicebus.audit.init-windows:4
docker pull particular/servicecontrol.azureservicebus.audit-windows:4
docker pull particular/servicepulse-windows:1
docker-compose up --detach %*
