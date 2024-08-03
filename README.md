# Health Med
Plataforma de agendamento de consultas medicas, desenvolvida em .NET 8 com arquitetura de microsserviços e arquitetura limpa com testes unitários e testes de integração.

## 📋 Pré-requisitos

* Azure Service Bus
* Azure SQL Database
* Azure Kubernetes Service (AKS)
* Docker Hub

## Integrantes

- [João Gasparini](https://github.com/joaogasparini)
- [Lucas Hanke](https://github.com/lucasbagrt)
- [Victoria Pacheco](https://github.com/vickypacheco)
- [Rafael Araujo](https://github.com/RafAraujo)

### Este projeto contém dois microserviços: `User API` e `Appointment API`.
### Abaixo estão as instruções informativas para configurar e implantar esses serviços na Azure, incluindo configuração do Service Bus, banco de dados SQL, AKS, e pipeline de CI/CD para Docker Hub, AKS e análise de código com Sonar Cloud.

## Configuração do Service Bus

- Crie um namespace do Service Bus na Azure.
- Crie uma fila no Service Bus.
- Obtenha a string de conexão do Service Bus e adicione-a no `appsettings.json` da `Appointment API`:
![image](https://github.com/user-attachments/assets/57bc9ef6-7625-41be-bb33-043773f20eb3)


## Configuração do Banco de Dados SQL

- Crie um serviço de banco de dados SQL na Azure.

- Obtenha a string de conexão do banco de dados e adicione-a no appsettings.json da Appointment API e User API:
![image](https://github.com/user-attachments/assets/99d9f42c-7713-465d-9f5d-d6881fda6445)

## Configuração do AKS

- Crie um cluster do AKS na Azure.
- Implante a User API no AKS.
- Obtenha a URL externa do serviço User API e adicione-a no appsettings.json da Appointment API:
![image](https://github.com/user-attachments/assets/a6cfab33-0a92-47d3-bcfd-5134ad55b017)

- Implante a Appointment API no AKS.

## Pipeline de CI/CD
- Um pipeline de CI/CD já está configurado no repositório para automatizar o processo de criação de imagens Docker e implantação no AKS:

- Build: Cria imagens Docker para User API e Appointment API e publica-as no Docker Hub.
- Deploy: Implementa as imagens no AKS.
![image](https://github.com/user-attachments/assets/100126b8-c769-44bb-856b-f8ae72b9b896)

### Integração com SonarCloud
- A análise de código estático usando SonarCloud já está configurada no pipeline de CI/CD para a branch main. O SonarCloud ajuda a garantir a qualidade do código, identificando bugs, vulnerabilidades e code smells.
