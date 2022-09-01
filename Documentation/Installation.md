# Installation

## <a name="software"></a>Software

The following software are require to starting using this server:

- [.Net 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker](https://www.docker.com/products/docker-desktop)

## Docker Compose

docker-compose for the server dependents

```yaml
version: '3'
services:
  mysql:
    container_name: mysql
    command: --default-authentication-plugin=mysql_native_password
    image: mysql:8.0
    environment:
      MYSQL_ROOT_PASSWORD: root
      TZ: Africa/Lagos
    ports:
      - '3306:3306'
    volumes:
      - './data/mysql:/var/lib/mysql/'
  memcached:
    container_name: memcached
    image: memcached:1.6.9-alpine
    ports:
      - '11211:11211'
```

## <a name="setup"></a>Setup

Create `appsettings.json` in `src/Presentation`. Copy the content in `appsettings.json.dist` into `appsettings.json` and set the appropriate values.

Note: do **NOT** add production values to `appsettings.json.dist`. It will be added to git history, moreover do **NOT** delete `appsettings.json.dist`.

## <a name="docker"></a>Docker Image

Run `./build.sh` to build the docker image on your local machine or run `docker pull ghcr.io/dudewith3faces/crowdbux_backed:main` for the latest official release

Docker images are tagged based on branch name, _main_, _qa_, and _dev_;
