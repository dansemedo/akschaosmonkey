# akschaosmonkey
An Azure Function that receives a POST request and interact with Azure Kubernetes Services, in this first version you can delete all the pods from a deployment and scale the replica set to add 1 more pod. If you send a POST with the command DELETE will delete all pods and if you send the message with the command SCALE will add 1 more pod to the replica set.
