# Makefile para proyecto .NET (C#)
# Uso: make [build|publish|run|clean|help]

# Configuración
PROJECT_FILE = $(wildcard *.csproj)
PROJECT_NAME = $(basename $(PROJECT_FILE))
PUBLISH_DIR = ./publish
RUNTIME = linux-x64
TARGET_FRAMEWORK = net8.0

# Comandos principales
.PHONY: help build publish run clean

help:
	@echo "🎯 Makefile para proyecto .NET: $(PROJECT_NAME)"
	@echo ""
	@echo "📌 Comandos disponibles:"
	@echo "   make build        → Compila el proyecto"
	@echo "   make publish      → Publica un ejecutable independiente para Linux"
	@echo "   make run          → Ejecuta localmente (requiere SDK)"
	@echo "   make clean        → Limpia archivos generados"
	@echo "   make help         → Muestra esta ayuda"
build:
	@echo "🔨 Compilando el proyecto..."
	dotnet build $(PROJECT_FILE) -c Release --no-restore
	@echo "✅ Compilación completada."

publish:
	@echo "📦 Publicando como ejecutable independiente para $(RUNTIME)..."
	dotnet publish $(PROJECT_FILE) \
	-c Release \
	-r $(RUNTIME) \
	--self-contained true \
	-p:PublishSingleFile=true \
	-p:PublishTrimmed=false \
	-p:IncludeNativeLibrariesForSelfExtract=true \
	-p:ShowLinkerSizeComparison=true \
	-o $(PUBLISH_DIR) 

run:
	@echo "▶️  Ejecutando la aplicación..."
	#dotnet run  "10.79.6.247:1521/SIGEPROD.oncop.gob.ve" "Consulta" "pumyra1584" "http://localhost:8085" #--project $(PROJECT_FILE) --no-build
	dotnet run  #\
			#-dns "10.79.6.247:1521/SIGEPROD.oncop.gob.ve" \
			#-url "http://localhost:5288" \
			#-user1 "Consulta" \
			#-pass1 "pumyra1584" \
			#-user2 "USR_INGREFIS" \
			#-pass2 "turgamar9648"
clean:
	@echo "🧹 Limpiando..."
	dotnet clean
	rm -rf $(PUBLISH_DIR)/
	rm -rf ./bin ./obj
	dotnet restore
	@echo "✅ Limpieza completada."

install:
	# Importar la clave GPG de Microsoft
	wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
	sudo dpkg -i packages-microsoft-prod.deb
	rm packages-microsoft-prod.deb
	# Actualizar paquetes
	sudo apt update

	# Instalar SDK de .NET 8.0
	sudo apt install -y apt-transport-https
	sudo apt update
	sudo apt install -y dotnet-sdk-8.0
	dotnet --version
	# Atajo: make → help


installdb:
	dotnet add package Microsoft.EntityFrameworkCore --version 7.0.10
	dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 7.0.10
	dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.10
	dotnet restore

deletedb:
		rm -rf Migrations
		rm app.db

migrate:
	dotnet ef migrations add InitialCreate 
	dotnet ef database update

test:
	dotnet test


install:
	dotnet add package AutoMapper #--version 15.0.1
	dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection #--version 12.0.1
	dotnet add package BCrypt.Net-Next #--version 4.0.3
	dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer #--version 8.0.8
	dotnet add package Microsoft.AspNetCore.OpenApi #--version 8.0.8
	dotnet add package Microsoft.EntityFrameworkCore #--version 9.0.7
	dotnet add package Microsoft.EntityFrameworkCore.Design #--version 9.0.7
	dotnet add package Microsoft.EntityFrameworkCore.Sqlite #--version 9.0.7
	dotnet add package Microsoft.NET.ILLink.Tasks #--version 8.0.18
	dotnet add package Oracle.ManagedDataAccess.Core #--version 23.9.1
	dotnet add package Swashbuckle.AspNetCore #--version 6.5.0
	dotnet add package Swashbuckle.AspNetCore.Annotations #--version 9.0.3
	dotnet add package System.IdentityModel.Tokens.Jwt #--version 8.13.0



#dotnet new webapi -n backend_ont_2
all: help

# Agrega los paquetes necesarios
#dotnet add package Microsoft.EntityFrameworkCore
#dotnet add package Microsoft.EntityFrameworkCore.Sqlite
#dotnet add package Microsoft.EntityFrameworkCore.Design
#dotnet add package Microsoft.EntityFrameworkCore.Tools
#dotnet add package Microsoft.EntityFrameworkCore
#dotnet add package Microsoft.EntityFrameworkCore.Design
#dotnet add package Microsoft.EntityFrameworkCore.Sqlite