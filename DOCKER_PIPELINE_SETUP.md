# GitHub Actions - Docker Hub Pipeline Setup

## Configuración de Secrets en GitHub

Para que la pipeline funcione correctamente, necesitas configurar los siguientes secrets en tu repositorio de GitHub:

### Pasos para agregar secrets:

1. Ve a tu repositorio en GitHub: `https://github.com/ebugedo/tfs-api-sample`
2. Haz clic en **Settings** (Configuración)
3. En el menú lateral izquierdo, selecciona **Secrets and variables** → **Actions**
4. Haz clic en **New repository secret**
5. Agrega los siguientes secrets:

### Secrets requeridos:

#### `DOCKERHUB_USERNAME`
- **Nombre:** `DOCKERHUB_USERNAME`
- **Valor:** Tu nombre de usuario de Docker Hub (ejemplo: `ebugedo`)

#### `DOCKERHUB_TOKEN`
- **Nombre:** `DOCKERHUB_TOKEN`
- **Valor:** Tu token de acceso de Docker Hub

### Cómo crear un Access Token en Docker Hub:

1. Inicia sesión en [Docker Hub](https://hub.docker.com/)
2. Haz clic en tu avatar/nombre de usuario en la esquina superior derecha
3. Selecciona **Account Settings** (Configuración de cuenta)
4. Ve a la sección **Security** → **Access Tokens**
5. Haz clic en **New Access Token**
6. Dale un nombre descriptivo (ejemplo: `github-actions`)
7. Selecciona los permisos: **Read, Write, Delete** (recomendado) o **Read & Write**
8. Copia el token generado (solo se muestra una vez)
9. Pega este token en el secret `DOCKERHUB_TOKEN` en GitHub

## Funcionamiento de la Pipeline

La pipeline se ejecutará automáticamente en los siguientes casos:

### 1. **Push a las ramas principales:**
   - `main`
   - `develop`
   
   La imagen se construirá y publicará en Docker Hub.

### 2. **Tags con formato semántico:**
   - Ejemplo: `v1.0.0`, `v2.1.3`
   
   Se crearán tags correspondientes en Docker Hub.

### 3. **Pull Requests:**
   - La imagen se construirá pero NO se publicará (solo validación).

### 4. **Ejecución manual:**
   - Puedes ejecutar la pipeline manualmente desde la pestaña **Actions** en GitHub.

## Tags generados automáticamente

La pipeline generará los siguientes tags en Docker Hub:

- `latest` - Solo para la rama principal (main/develop según configuración)
- `develop` - Para commits en la rama develop
- `main` - Para commits en la rama main
- `v1.0.0` - Para tags semánticos
- `1.0` - Versión major.minor
- `1` - Versión major
- `develop-sha-abc1234` - SHA del commit

## Personalización

Si necesitas cambiar el nombre de la imagen Docker o ajustar la configuración, edita las siguientes variables en `.github/workflows/docker-publish.yml`:

```yaml
env:
  DOCKER_IMAGE_NAME: tfsapi  # Cambia esto al nombre que desees
  DOCKERFILE_PATH: ./src/TfsApi/WebApi/Dockerfile
  DOCKER_CONTEXT: .
```

## Verificación

Una vez configurados los secrets:

1. Haz un push a la rama `develop` o `main`
2. Ve a la pestaña **Actions** en tu repositorio de GitHub
3. Verás el workflow ejecutándose
4. Cuando termine, verifica en Docker Hub que la imagen fue publicada

## Imagen resultante

Tu imagen estará disponible en:
```
docker pull <tu-usuario-dockerhub>/tfsapi:latest
```

## Uso local

Para probar la imagen localmente:

```bash
# Descargar la imagen
docker pull <tu-usuario-dockerhub>/tfsapi:latest

# Ejecutar el contenedor
docker run -p 8080:80 <tu-usuario-dockerhub>/tfsapi:latest
```

## Soporte multi-plataforma (Opcional)

Si necesitas soporte para múltiples arquitecturas (ARM64, AMD64), edita la línea `platforms` en el workflow:

```yaml
platforms: linux/amd64,linux/arm64
```

## Troubleshooting

### Error: "unauthorized: authentication required"
- Verifica que `DOCKERHUB_USERNAME` y `DOCKERHUB_TOKEN` estén configurados correctamente
- Asegúrate de que el token no haya expirado

### Error: "repository does not exist"
- El repositorio se creará automáticamente en el primer push
- Asegúrate de que tu usuario de Docker Hub tenga permisos de escritura

### La pipeline no se ejecuta
- Verifica que el archivo esté en `.github/workflows/docker-publish.yml`
- Revisa que los triggers (on:) coincidan con tu caso de uso
