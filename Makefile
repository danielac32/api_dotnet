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
	-p:PublishTrimmed=true \
	-p:IncludeNativeLibrariesForSelfExtract=true \
	-o $(PUBLISH_DIR) \
	--no-build
	@echo "✅ Publicado en: $(PUBLISH_DIR)/$(PROJECT_NAME)"

run:
	@echo "▶️  Ejecutando la aplicación..."
	dotnet run #--project $(PROJECT_FILE) --no-build

clean:
	@echo "🧹 Limpiando..."
	dotnet clean
	rm -rf $(PUBLISH_DIR)/
	rm -rf ./bin ./obj
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
migrate:
	dotnet ef migrations add InitialCreate
	dotnet ef database update

test:
	dotnet test
 
all: help

# Agrega los paquetes necesarios
#dotnet add package Microsoft.EntityFrameworkCore
#dotnet add package Microsoft.EntityFrameworkCore.Sqlite
#dotnet add package Microsoft.EntityFrameworkCore.Design
#dotnet add package Microsoft.EntityFrameworkCore.Tools
#dotnet add package Microsoft.EntityFrameworkCore
#dotnet add package Microsoft.EntityFrameworkCore.Design
#dotnet add package Microsoft.EntityFrameworkCore.Sqlite