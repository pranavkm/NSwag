# Analyzers for Mvc

## Async void actions \ handlers
We've had more than a handful of issues filed around this, more commonly with Razor Pages. Handlers or actions are marked `async void` and this results in spurious behavior.

**Input**
```C#
public async void SomeMethod()
{
    ...
}
```

**CodeFix**
```C#
public async Task SomeMethod()
{
    ...
}
```

**Considerations**: None

**Applies to**: As a warning on all controller actions, page handlers

--------------------------

## Verify correctness of attribute routes 
Along the lines of the logging analyzer - https://github.com/aspnet/Logging/blob/dev/src/Microsoft.Extensions.Logging.Analyzers/LogFormatAnalyzer.cs. 

Attribute routes are stringly typed and could be malformed. This would of course fail at runtime. At minimum, we can display warnings that say the route is malformed. Providing a code fix for this is difficult, this would just be an analyzer.

**Applies to**: All controller actions.

--------------------------

## Antiforgery on controller actions with form inputs
Razor Pages adds an implicit antiforgery validation filter to all pages. The intent is that if you're accepting form inputs, you'll need antiforgery validation. We could provide a code fix for POST controller actions that accept complex types without a binding source attribute and do not have a antiforgery validation filter.

**Input**
```C#
[HttpPost]
public IActionResult EditUser(UserModel model)
{
    ...
}
```

**CodeFix**
```C#
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult EditUser(UserModel model)
{
    ...
}
```

**Applies to**: Non-Api controller POST actions

**Considerations**: We may need to give additional guidance on crafting the form correctly (i.e. use FormTagHelper \ BeginForm etc)

--------------------------


# ApiController analyzers

ApiControllers are opinionated and offer opportunities to give diagnoses when writing code instead of having runtime exceptions. The intent is to encourage the user to have a controller with minimal Mvc specific cruft.

## ApiController actions must be attribute routed
ApiControllers require all actions to be attribute routed. The analyzer can detect if neither the
controller nor the action sets up an attribute route and recommend setting up one. There's room for some magic to happen in constructing the route as part of the fix and determining what type of http method the action is.

**Input**
```C#
[ApiController]
public class UserController
{
    public IActionResult EditUser([FromRoute] int id, UserModel model)
    {
        ...
    }
}
```

**CodeFix**
```C#
[ApiController]
public class UserController
{
    [HttpPost("[controller]/[action]/{id}")]
    public IActionResult EditUser([FromRoute] int id, UserModel model)
    {
        ...
    }
}
```

**Considerations**: There's lots of room to decide  what attribute needs to be placed where, how we construct the route. In particular if a controller with many actions does not have any attribute routes, there's possibly room to consolidate and a single `[Route]` on the controller. E.g.

**Input**
```C#
[ApiController]
public class UserController
{
    public IActionResult GetUser([FromRoute] int id)
    {
        ...
    }

    public IActionResult EditUser([FromRoute] int id, [FromBody] UserModel model)
    {
        ...
    }
}
```

**CodeFix**
```C#
[ApiController]
[Route("[controller]/[action]/{id}")]
public class UserController
{
    [HttpGet]
    public IActionResult GetUser([FromRoute] int id)
    {
        ...
    }

    [HttpPost]
    public IActionResult EditUser([FromRoute] int id, [FromBody] UserModel model)
    {
        ...
    }
}
```
This is clearly *cleaner*, but it likely harder to pull off.

**Applies to**: All ApiController actions

-------

## ApiController actions do not require `FromBody` \ `FromRoute` \ `FromQuery` on parameters if these are deterministic

ApiControllers calculate the binding source for action parameters that do not specify a [From*] attribute:
* Complex types are assumed to be FromBody
* Parameters that appear in all routes are FromRoute
* All other parameters default to FromQuery.

We could write an analyzer \ code fix that suggests removing these attributes:


**Input**
```C#
[HttpGet("{id}")]
public IActionResult GetUser([FromRoute] int id)
{
    ...
}
```

**CodeFix**
```C#
[HttpGet("{id}")]
public IActionResult GetUser(int id)
{
    ...
}
```

**Input**
```C#
[HttpPut]
public IActionResult Put([FromBody] Pet pet)
{
    ...
}
```

**CodeFix**
```C#
[HttpPut]
public IActionResult Put(Pet pet)
{
    ...
}
```

-------

## ApiController actions should return ActionResult<T>
If an ApiController action returns `IActionResult` \ `ActionResult`, we could suggest replacing it with `ActionResult<T>` where the value of T is determined by inspecting the type of all returned ObjectResult.Object instances.

**Input**
```C#
[HttpGet("{id}")]
public async Task<IActionResult> GetPet(int id)
{
    var pet = await Repo.GetPetAsync(id);
    if (user == null)
    {
        return NotFound();
    }

    return Ok(pet);
}
```

**CodeFix**
```C#
[HttpGet("{id}")]
public async Task<ActionResult<Pet>> GetPet(int id)
{
    var pet = await Repo.GetPetAsync(id);
    if (user == null)
    {
        return NotFound();
    }

    return Ok(pet);
}
```


