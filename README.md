**Build image and run container:**  

docker build -t demoapp-image .  
docker run --name demoapp-container -d -p 8000:80 demoapp-image  

**Run application:** localhost:8000  

**Tag image and upload to Docker Hub:**  

docker tag demoapp-image bstraehle/mvc-app:latest  
docker push bstraehle/mvc-app:latest  
