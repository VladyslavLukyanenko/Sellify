#!/bin/bash
cd ../src/ProjectIndustries.Sellify.Infra && \
 dotnet ef migrations add Initial --startup-project ../ProjectIndustries.Sellify.WebApi/