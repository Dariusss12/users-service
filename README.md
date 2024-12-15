## Prerequisites

- SDK [.NET 8](https://dotnet.microsoft.com/es-es/download/dotnet/8.0).
- Port 5001 Available
- git [2.33.0](https://git-scm.com/downloads) or higher.
- [Docker](https://www.docker.com/) **Note**: If you do not have installed is highly recommended to see the steps to correctly install docker on your machine with [Linux](https://docs.docker.com/desktop/install/linux-install/), [Windows](https://docs.docker.com/desktop/install/windows-install/) or [MacOs](https://docs.docker.com/desktop/install/mac-install/).
- [rabbitMQ](https://www.rabbitmq.com/docs/download)
- [Cubi12 Legacy](https://github.com/Dizkm8/cubi12-api) **Note**: If you have already installed this project, it is not necessary to install it again.

## Getting Started

Follow these steps to get the project up and running on your local machine:

1. Open Docker and Install rabbitMQ instance.
     **Note**: If you have already running an instace, you can skip this step.
     ```bash
     docker run -d --hostname my-rabbit --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
     ```    

3. Clone the repository to your local machine.
     ```bash
     git clone https://github.com/Dariusss12/users-service
     ```

4. Navigate to the root folder.
     ```bash
     cd user-service
     ```
   
5. Inside the folder user-service create a file and call it .env then fill with the following example values.
      ```bash
      DB_CONNECTION="Host=your_postgre_host;Port=5432;Database=your_postgre_database;Username=your_postgre_user;Password=your_postgre_password
      JWT_SECRET=your_super_secret_key
      ```
    
6. Install project dependencies using dotnet sdk.
     ```bash
     dotnet restore
     ```
7. Run project.
      ```bash
      dotnet run
      ```

This will start the development server, and you can access the app in your web browser by visiting http://localhost:5000.

## Use

You can use [Postman](https://www.postman.com/) or [API Gateway](https://github.com/Dariusss12/api-gateway) to use the API.

