# FeedTrac

FeedTrac is our (Group 12) submission for Team Software Engineering Assignment 3. It is a feedback tracking sevice that aims to allow students to submit feedback to their instructors. This software is merely in its prototype phase.

## Setup

In order for FeedTrac to function, you must first create the database on your local machine, the following script sets up a docker container which can be used for FeedTrac.

```cmd
docker run -d --name FeedTrac -e POSTGRES_PASSWORD=Xd12a4kxoNX6bZu6x3vo568v -p 5432:5432 -v feedtrac_data:/var/lib/postgresql/data postgres:latest
```

For full functionality, including the email and AI assistant service, please create a .env file in FeedTrac.server with the following info

```env
GMAIL_EMAIL=
GMAIL_PASS=
GEMINI_KEY=
```

Note for our supervisor: This .env file will be included with the submission. **The keys provided with the submission will only be valid up until that the grade for this project has been ratified, after this day they will all be disabled**

## Running the Project

To run the project, please load the .sln file in your C# IDE of choice, select FeedTrac: HTTP as your startup config (note HTTPS is not setup with a certificate), and click start.

On first run, the solution will generate the default administrator file, its details can be changed in program.cs however by default it will be set to this:

```
email=feedtrac-admin@lincoln.ac.uk
password=Password123!
2FA_secret=VMNAYBBTP4PHHMNF53O2W5UGRJDD442G // - Input into your TOTP authenticator app of choice
```

the website can then be opened at localhost:5173