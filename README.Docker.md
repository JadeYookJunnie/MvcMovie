### Building and running your application

When you're ready, start your application by running:
`docker compose up --build`.

Your application will be available at http://localhost:5000.

### Deploying your application to the cloud

First, build your image, e.g.: `docker build -t myapp .`.
Then, push it to your registry, e.g. `docker push myregistry.com/myapp`.