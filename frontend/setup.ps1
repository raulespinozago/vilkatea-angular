Param(
  [string]$AppName = "cliente-app",
  [string]$ApiUrl = "https://localhost:7080/api"
)

Write-Host "==> Generando proyecto Angular '$AppName'..." -ForegroundColor Cyan
if (-Not (Test-Path $AppName)) {
  ng new $AppName --routing --style=scss --skip-git
} else {
  Write-Host "Carpeta $AppName ya existe, se omitirá 'ng new'." -ForegroundColor Yellow
}

Push-Location $AppName

Write-Host "==> Instalando Angular Material..." -ForegroundColor Cyan
ng add @angular/material -y

Write-Host "==> Generando módulo/recursos de clientes..." -ForegroundColor Cyan
ng g m features/clients --route clientes --module app.module
ng g c features/clients/pages/lista-clientes --skip-tests
ng g c features/clients/pages/editar-cliente --skip-tests
ng g s features/clients/services/clientes --skip-tests
New-Item -Path "src/app/features/clients/models" -ItemType Directory -Force | Out-Null

# Copiar archivos provistos
$root = Split-Path $PSScriptRoot -Leaf
Copy-Item -Path "$PSScriptRoot\src_overrides\*" -Destination "src" -Recurse -Force

# Reemplazar environment.api
(Get-Content "src/environments/environment.ts") -replace 'api: .+','api: "'+$ApiUrl+'"' | Set-Content "src/environments/environment.ts"

Write-Host "==> Listo. Arranca con 'ng serve -o'." -ForegroundColor Green

Pop-Location
