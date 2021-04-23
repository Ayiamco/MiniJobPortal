# MiniJobPortal 

This is a website for a hypothetical company that contains the companies landing page while also adding functionality to upload job posting and review job aplications. 

#### The job posting portal has the following features
> - Authentication for admin and jobapplicants.
> - Admins can upload new job openings with a a deadline for applicants.
> - Admins can end Job application before the initally specified deadline.
> - Admin can gets email notification whenever an applicants applies for a job.
> - Applicants can  upload their resume and profiles.
> - Admins can reject or accept applicants.
> - Applicants gets email notification whenever admin acts on  their application.
> - Admin gets a dashboard to view job applicantion summary.

#### Dependencies
The project was built using .net framework 4.72 and all the packages used can be found in the **packages.config** file in the root directory.
#### Installation instructions
After cloning the project you should do the following to get the project running smoothly.
- The mail service in the project uses sendgrid hence you would need to create a sendGrid account if you do not have one to get your api key.
- Rename the **appDemo.config** file in the root directory to **app.config**
- Edit the app.config file and and change the values of the apiKey, user and userEmail to the corresponding correct values of your send grid account.
- Add a value for the adminEmail and adminPassword in the app.config file . The adminEmail would be the email that recieves notification whenever
an applicant applies for a job.
- The admin dashboard route can be found at **/admin**