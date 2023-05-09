# UtilityComplaints

Utility Complaints App is a learning project for GeeksForLess .NET Course

This application allows users create complaints about territory improvements. Registered users can create new complaint to utility services with exact geolocation, description (Graffiti on fences, broken bench, trashed territory) and media files (optionally). Complaint get status “Active” until the representative of utility services responds to it with commentary (Territory was cleaned, graffiti was washed off from fence, bench is repaired etc.) and changes status to “Solved”

There’s 2 type of users: 
•	Regular users can create complaints and view complaints of others 
•	Service Representatives can respond to complaints of entrusted district and change it’s status

Application has been localized to en, pl, ua, cz, expanding page coverage.

Application contains simple implementation of SignalR chat

To add:
•	Services should be divided by districts viewing and solving only complaints of entrusted district, e.g. Service account of Dniprovskyi district can see only complaints within Dniprovskyi district (need to figure out how to reverse geocode district from coordinates)
•	Like system to vote up Complaints thet already exist to increase their priority

This section will be updated 