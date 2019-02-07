# AKS Chaos Monkey

An Azure Function that receives a POST request and interact with Azure Kubernetes Services.

In this first release you can delete all the pods from a deployment and scale the replica set to add 1 more pod. 

## Features of the AKS Chaos Monkey v 1.0 (RELEASED)

- POST to Azure Function with the message "command" : "scale" to add 1 more pod to the replica set.
- POST to Azure Function with the message "command" : "delete" to delete all the pods in the replica set.

## Features of the AKS Chaos Monkey v 1.1 (WORK IN PROGRESS)

- Configure your .kube conf in Azure Key Vault to secure your AKS config file.
