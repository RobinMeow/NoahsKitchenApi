# NoahsKitchen API

## (Layered) Architecture

### Application Layer (Controllers)

Responsible for (no pun intended):

- listening to HTTP Requests and giving HTTP Responses.
- mapping to and from DTOs for HTTP communication.
- delegating work to the Domain Layer.

Normally, the flow of a method within a Controller will look like this:

#### Example HttpPost

1. Receive HttpRequest (body with DTO)
2. Validate the DTO using something very similar to the Specification Pattern [Specification Pattern Wikipedia](Specification Pattern)
   - If invalid, return the "error messages" in a `BadRequest` (not sure if this is the correct one to use) via the [Specifications (Eric Evans and Martin Fowler)](https://martinfowler.com/apsupp/spec.pdf)
3. Delegate the work to the lower layers (Domain Layer and Infrastructure)
4. Return the newly created model in a HttpResponse (body)

### Domain Layer

I'm not going to explain what a Domain is. It doesn't matter anyways since this is only a simple CRUD application.

If you want to learn more about DDD (Domain-Driven Design), there is a short summary from 2015 called [Domain-Driven Design Reference - Definitions and Pattern Summaries](https://www.domainlanguage.com/wp-content/uploads/2016/05/DDD_Reference_2015-03.pdf), or you can read the ['big blue book' by Eric Evans](https://www.amazon.de/Domain-Driven-Design-Tackling-Complexity-Software/dp/0321125215/ref=sr_1_1?__mk_de_DE=%C3%85M%C3%85%C5%BD%C3%95%C3%91&crid=13DX941RWJJ3&keywords=Domain+Driven+Design&qid=1686647527&sprefix=domain+driven+desig%2Caps%2C109&sr=8-1) or the ['big red book' by Vaughn Vernon](https://www.amazon.de/Implementing-Domain-Driven-Design-Vaughn-Vernon/dp/0321834577/ref=pd_bxgy_img_sccl_2/258-2676143-5713501?pd_rd_w=8pPzi&content-id=amzn1.sym.1fd66f59-86e9-493d-ae93-3b66d16d3ee0&pf_rd_p=1fd66f59-86e9-493d-ae93-3b66d16d3ee0&pf_rd_r=9FWZ16J9515FK36S1DMR&pd_rd_wg=6Q5s0&pd_rd_r=feefa4a8-a1aa-4575-b659-e51200c7b5a6&pd_rd_i=0321834577&psc=1). There is also an (unfinished) GitHub example [IDDD_Samples (Vaughn Vernon using .NET)](https://github.com/VaughnVernon/IDDD_Samples_NET/tree/master) and the [Java Version of it](https://github.com/VaughnVernon/IDDD_Samples).

> Essentially, any Business Logic remains here. This is what the actual application 'is about'.

Also, the Domain is responsible for every data-related problem.
Meaning, IDs and dates will all be generated here, not in the frontend, nor the database. (If think this has obvious reasons and advantages I don't need to point out)

### Infrastructure Layer

Third-party Libraries/Frameworks/etc. will have their `ConcreteImplementation` here.

The ignorant Interface belongs to the Domain Layer.

## Common

This "Library" (or folder in this case :D) is literally just a shared module.

## Development

When you run `dotnet run`, you can navigate to `http://localhost:5126/Swagger` to test the API without UI.

## Deployment

To deploy your API, follow these steps:

- Copy the `appsettings.Template.json` file and rename it as `appsettings.Production.json` in your local development environment.
- Replace the sample data in the `appsettings.Production.json` file with your own sensitive information.

Initially, I attempted to use `appsettings.json` as the default name and file for my production values. However, this approach caused unexpected issues. The `AllowedOrigins` values became mixed up, and ASP.NET Core included both a local and production URL in a single string array, while the second production URL was missing for unknown reasons. Therefore, I recommend adhering to Microsoft's "recommendation" as mentioned in the [official documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0).

> Take note of how they use capitalization for the words `Staging` and `Development`.

I may consider renaming my other template files to follow this convention so that the entire project aligns with Microsoft's standards. (Can you sense Microsoft's influence and their desire to impose their conventions on you, the way I do? :ape:)

### Docker

To simplify production deployment using Docker, utilize the `docker-compose.yml` file and execute `docker-compose build` instead of a lengthy `docker run` command.
Using `docker-compose` instead of `docker build .` is mandatory as the `docker-compose.yml` file contains crucial values.

- Copy the `docker-compose.template.yml` file and rename it as `docker-compose.yml` in your local development environment.
- Replace the sample data in your newly created `docker-compose.yml` file with your own sensitive information.
Remember not to track your newly created file `docker-compose.yml` in your GitHub repository (if you stick to the suggested names, you won't have to do anything, because it is already added in the `.gitignore`).
The `docker-compose.template.yml` is provided as a replica of the original file, with the sensitive information replaced by example values.

> Please note that when using the local containerized API version in the development environment, MongoDB will not function at all.
> This is because the Dockerfile **does not** install MongoDB locally within the container.

Go To Production:

1. Run `docker-compose build` and push your docker image to a public repository (I have used Docker HUB for this application) `docker push username/repositoryname`
2. Go to Google Cloud Run, create a service.
3. Enter a Service name. For example, let's name it `gkb-api-service`.
4. Select a region that is geographically close to your users. I recommend selecting Price Tier 1, if you want to stay free. (This can raise up costs, as google should have pointed out to you already by now. If, and only if you usage is to high, really high. Also I recieved a mail, assuring me, that google will rather than billing me, stop the service, and I still need to upgrade my Billing Acount to be billed.)
5. Provide the container image URL in the following format: `docker.io/username/repository:latest`.
6. Ingress control -> `All`
7. Authentication -> `Allow unauthenticated invocations`. Security is handled through the connection string, which includes a username and password.
8. Warning: Before clicking "Create," expand the "Container, Networking, Security" panel located above the Create button. Ensure that the container port is set to 8080 (which is the default as of writing this).
9. Click "Create"

> Note: If you understand the implications and associated costs, you can make additional changes to suit your needs.
>
> Important: Your Docker image is now publicly exposed. It is highly recommended to delete the image from the public repository as soon as the Cloud Run service is up and running. Alternatively, you can use Google's Container Registry or Artifact Registry for better security instead of a public Docker Hub.

## ToDo

- ~~Add Unit Tests.~~
- Make use of Mocking.
- Implement `UnitOfWork` for transactions (And maybe other things).
- Add authentication.
- Add email service (Firebase is for free, I think).
- Maybe use Domain Events for some things, like sending emails, but the Application Layer is sufficient for this.
- For more, [see Requirements](../README.md#requirements)
