Demo MVC app using demo REST API (???).  Implements CRUD operations via API calls.

Build image and run container:  
```
docker build -t mvcapp-image .  
docker run --name mvcapp-container -d -p 8000:80 mvcapp-image  
```
URL:  
```
localhost:8000  
```
Tag image and upload to Docker Hub:  
```
docker tag mvcapp-image bstraehle/mvc-app:latest  
docker push bstraehle/mvc-app:latest  
```
For Docker orchestration using this container, see https://github.com/bstraehle/docker.git#readme  
For Kubernetes orchestration using this container, see https://github.com/bstraehle/kubernetes.git#readme  
