# Basketball Notifier

A web scraper that can be used to send summaries of the days basketball games.  
Uses TickerQ for scheduling, MimeKit for email, and AgilityPack for scraping.

![Papercut](/Docs/Papercut.png)

# How to use

Generally this is developed to use the standard `appsettings.json` file for configuration, an example of which can be
found below:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "SMTP": {
    "Server": "localhost",
    "Port": "2525",
    "Username": "username",
    "Password": "superSecurePassword",
    "To": "person@example.com",
    "From": "basketball@example.com"
  },
  "Schedule": {
    "CronExpression": "0 0 16 * * *"
  }
}
```

For testing, it's suggested to use Papercut.  
This has been used from Linux in a docker container, and the default configuration will work with this nicely:

```bash
docker pull changemakerstudiosus/papercut-smtp:latest
docker run -d -p 37408:8080 -p 2525:2525 changemakerstudiosus/papercut-smtp:latest
```

To execute the job on demand POST to the `/api/basketball/scrapenow` endpoint. E.g.

```powershell
Invoke-WebRequest -Method Post -Uri 'http://localhost:5000/api/basketball/scrapenow'
```

# Requirements

- [X] Read sports data from a website once a day and send it to a specific e-mail address
- [X] The data is to be collected from the reference basketball website
- [X] No interaction with the program required
- [X] Use the Agility Pack library for scraping 

## Stretch Goals

- [X] Use TickerQ for scheduling

# Features

- Run on a schedule or on-demand from an api call.  
- Scrapes the defined basketball webpage for todays scores
- Sends a scheduled email with the days results
- Supports email encryption
- Supports plaintext and HTML email clients

# Challenges

This one was relatively straightforward.  
Main challenges came from the fact that TickerQ changed version relatively recently and so a number of examples were 
out of date.
As TickerQ is almost entirely async it was difficult to know how best to configure a scheduled job from configuration.
Using options in the constructor felt like a bad idea.
This is definitely something I want to come back to.  
Additionally it wasn't immediately obvious how I should be passing in configuration for the SMTP client. Once I had 
found the Options pattern it was significantly easier. However, I wasn't really sure what I was searching for.

# Lessons Learned

- Learned all about scheduling
- Learned about scraping
- Learned a lot more about configuring DI

# Areas to Improve

- I'd like to make use of TickerQ more
- I would like to investigate some of the other schedulers like Quartz
- Better ways of generating HTML content for email
  - Also how to do any of the styling... 

Otherwise, I'm pretty content with the outcome of this project!

# Resources Used

- The C Sharp Academy
- Microsoft Documentation
- TickerQ
- MimeKit
- Agility Pack
- Stack Overflow
