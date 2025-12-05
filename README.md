# K8sCronWorker POC

This project is a Proof of Concept (POC) of a .NET Core Worker Service designed to run scheduled tasks, orchestrated by Kubernetes.

## Functionality

The worker service runs a simple task every minute. The task logs a message to the console. This demonstrates a basic cron job functionality within a .NET application.

## How it Works

The application uses the `Cronos` library to parse a cron expression and schedule the execution of a task. The `Worker` class, which inherits from `BackgroundService`, contains the core logic for the scheduling and execution of the task.

The cron expression is defined as a constant in the `Worker.cs` file:

```csharp
private const string CronString = "*/1 * * * *"; // Every 1 minute
```

The `ExecuteAsync` method calculates the next occurrence of the cron expression and waits until that time to execute the task.

## Prerequisites

To build and run this project, you will need the following:

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [Docker](https://www.docker.com/get-started)
*   [Kubernetes](https://kubernetes.io/docs/setup/) (e.g., Minikube, Docker Desktop with Kubernetes)

## How to run locally

1.  Clone this repository.
2.  Navigate to the `K8sCronWorker` directory.
3.  Run the following command:

```bash
dotnet run
```

This will start the worker service, and you should see log messages in the console every minute.

## How to build and deploy to Kubernetes

### Building the Docker image

From the root of the project, run the following command to build the Docker image:

```bash
docker build -t k8scronworker:v1 .
```

### Deploying to Kubernetes

The provided `deployment.yaml` file describes a Kubernetes `Deployment` to run the worker.

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: cron-worker-deployment
  labels:
    app: cron-worker
spec:
  replicas: 1 # Only 1 replica to avoid duplicate tasks!
  selector:
    matchLabels:
      app: cron-worker
  template:
    metadata:
      labels:
        app: cron-worker
    spec:
      containers:
      - name: cron-worker
        image: k8scronworker:v1 # Name of the image we created
        imagePullPolicy: IfNotPresent # Use local if it exists
        resources:
          limits:
            memory: "128Mi"
            cpu: "250m"
```

To deploy, run the following command:

```bash
kubectl apply -f deployment.yaml
```

This will create a single pod running the cron worker.
