# Noah's Kitchen (Subject to change)

Welcome to Gemeinschaftskochbuch, a community cookbook project where users can share and explore recipes within a specific community (e.g., family and friends).

## Projects

- [ASP.NET Core API](api/README.md) - Backend.
- [ASP.NET Core Unit Tests](api-tests/README.md) - Unit Tests.
- [Angular](ui/README.md) - Frontend.
- [Seed MongoDB](seed-mongo-db/README.md) - seeding script for local development.

> For development notes, refer to [Development Notes](development-notes.md).

## Requirements

- The app should recalculate quantities based on the desired *number of dishes* selected by the user.
- A *Recipe* is considered unique based on the combination of *Author* and *RecipeName* (composite key).
- *Ingredients* should be reusable to avoid different spellings for the same *ingredient*. When adding an *Ingredient* to a *Recipe*, the user should be able to select existing *ingredients*. If there is no existing one, it will create a new *ingredient* automatically. (In a later version it might be desired, to have a way of merging ingredients. If there were duplicates created with different spellings.)
- *Recipes* should be *categorizable* or *taggable* for better *search*- and *filtering*.
- A *Recipe* can contain *multiple descriptions **and** notes*, which should be displayed accordingly. The ability to reorder the display of *descriptions*, *notes* and *ingredients* through drag and drop may be desirable.
- The frontend only needs to support the German language; internationalization is not required. (Multiple languages would cause trouble anyways because our culinary skills are multicultural.)
- Access to the app should be restricted to a specific group of people using a provided "key".
- Each user should have the ability to *exclude recipes* from *search results* based on the *recipe's author*. This setting should not be visible to other users.
- Consider implementing a *Like/Dislike system* for recipes. Only positive likes should be public, while dislikes remain for private use.

> Note/ToDo: There are additional requirements documented in Confluence that need to be added here. Big sorry.

## Project Structure

The projects share the same Git repository for convenience, but they are still decoupled. The exception is the seeding Express app, which may contain references (relative paths) to the (Angular)TypeScript models to facilitate automated seeding in the future.

The Source Code contains a bunch of links, refering to external site, like MSDN or MongoDB. Usually provided as a source for reasonings behind decisions. This should leave the source code (before deployment) at some point and move into the Confluence.

Keep all commits you do, always prefer fast-forward merge into dev. Only merge dev into main, using squash, forming a single commit describing the change.
Keep the commit references in the commit message (they refer to the commit hashes in dev).

---

## Motivation

One part is being able to share recipes with my family and friends easily.

Another part is personal growth through learning various up-to-date technologies that I haven't had experience with. I intend to implement the entire infrastructure as a learning exercise. Additionally, I want to address any unknowns early in the project to avoid getting stuck during development. I point this out, because this does not reflect the way I usually would start a new project.

I might even explore build pipelines using services like ~~Azure or Bitbucket~~ (most likely) Jenkis, although the availability of free options is essential for this project.

## License

Gemeinschaftskochbuch is available under the [MIT license](LICENSE).

## Contact

Feel free to reach out to me by sending an email to [robinmeow97@gmail.com](mailto:robinmeow97@gmail.com).

If you appreciate my efforts and would like to support me, you can show your appreciation by [buying me a coffee](https://ko-fi.com/ribyn)! ☕️

> Just as Jesus broke bread with his disciples, may you find fellowship and connection in sharing meals, and may your culinary endeavors be a reflection of love and unity.
