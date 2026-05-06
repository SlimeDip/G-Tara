<div align="center">

<h1 align="center">G-Tara — Gala Planning Assistant</h1>
G-Tara is a Windows Forms application for planning and coordinating galas. Hosts can schedule events, manage participants and availability, select locations, and share plans with attendees.

</div>

## Overview
In G-Tara, users organize a gala from start to finish. They pick dates, locations, and participants, then finalize a plan with weather-aware tips and optional email updates.

**Highlights:**
- Gala scheduling with date ranges
- Participant management with availability
- Location search and map pinning
- Weather lookup for the event date
- Optional email dispatch to Gmail participants

---

## UML Diagram
![UML_Diagram](static/UML.png "UML_Diagram")

## Features and Functionalities of the System
- Create, edit, and delete galas with date ranges and plans.
- Manage participants and their availability.
- Search locations and pick coordinates via an embedded map.
- Fetch and display weather data for the scheduled date.
- Send plan emails to Gmail participants.

## Explanation of How the Program Works
- The main window lists galas and lets the user add, edit, or delete them.
- Add/Edit Gala handles date selection, location lookup, and participant selection.
- Services persist data to JSON files and call external APIs for weather and location search.
- Participants are managed in a dedicated dialog with availability selection.

## OOP Principles Used
### Encapsulation
- `Gala` and `Person` keep related state and rules together (e.g., availability checks).
- External code interacts with public methods like `IsAvailableOn()` rather than modifying internal logic.

```csharp
public abstract class Person
{
	public List<DateTime> AvailableDates { get; set; } = new();
	public DateTime AvailableStartDate { get; set; }
	public DateTime AvailableEndDate { get; set; }

	public bool IsAvailableOn(DateTime date)
	{
		if (AvailableDates.Count > 0)
		{
			return AvailableDates.Any(d => d.Date == date.Date);
		}

		if (AvailableStartDate != default || AvailableEndDate != default)
		{
			return AvailableStartDate.Date <= date.Date && AvailableEndDate.Date >= date.Date;
		}

		return false;
	}
}
```

### Abstraction
- `Person` defines shared data and behavior for different people types.
- `Participant` and `Host` reuse the same base model while focusing on their own roles.

```csharp
public abstract class Person
{
  public string Name { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;

  public bool IsAvailableOn(DateTime date)
  {
    return true;
  }
}

public class Participant : Person
{
    // code n stuffs
}

public class Host : Person
{
    // code n stuffs
}
```

### Inheritance
- `Participant` and `Host` inherit from `Person`, sharing identity and availability fields.

```csharp
public class Participant : Person
{
	public string AvailableDatesDisplay { get; }
}

public class Host : Person
{
	public List<string> ManagedGalaIds { get; set; } = new();
}
```

### Polymorphism
- The app treats different people types through the shared `Person` base type and runs the same logic against them.

```csharp
public List<Person> GetAvailableAttendees()
{
	return GetAllAttendees()
		.Where(p => p.IsAvailableOn(ScheduledDate))
		.ToList();
}
```

## Instructions on How to Run the Application
1. Open the solution in Visual Studio.
2. Restore NuGet packages if prompted.
3. Set environment variables as needed:
   - `OPENWEATHER_API_KEY` for weather data.
   - `GMAIL_USER` and `GMAIL_PASS` for email sending.
   - Optional: `SAVE_DIR` to override the default save folder.
4. Build and run the project.

## Names of the Developers or Team Members
- [Add developer names here]
