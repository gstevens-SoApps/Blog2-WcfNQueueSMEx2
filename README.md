# Blog2-WcfNQueueSMEx2
An SO App is a Service Oriented App.

Code for the blog article "SO Apps 2, WcfNQueueSMEx2 – To be Determined" at  https://dotnetsilverlightprism.wordpress.com/2015/05/12/TobeDetermined/

*  THIS Iteration 2 PROJECT IS CURRENTLY UNDER CONSTRUCTION and may not work!!  The blog article has not yet been written.

*  Please read the Setup Instructions document for how to install ServiceModelEx and setup Azure to support this code.
*  
ITERATION 2 PLAN
1. Almost DONE Design and implement a Visual Studio solution structure ammenable to service oriented apps -- A very flat structure so that the resueable components can easily be seen by scrolling, rather than expanding a deep hierarchy of folders that serves to hide components.  CURRENT -- Refining structure.
2. Almost DONE Design and implement the Data Feed subsystem per the IDesign Method, using Engines and ResourceAccessors to do the work, leaving only the orchestration of the work to the Manager code.  CURRENT -- Decoupling ResourcAccessor from specific persistence mechanisms via repositories.
3. NEW GOAL -- Develop and present system diagrams showing software structure.  CURRENT -- In progress.
3. DELAYED Stretch Goal -- Add a second Manager to demonstrate Manager to Manager interactions and more microservices.  This may not happen till Itertion 3.
4. NOTE -- 5-21-15  Solution Structure and Setup Instructions are almost there, but still have discrepancies.
5. NOTE -- 5-21-15  It now runs.
6. NOTE -- 5-28-15  This code passed a series of rigorous stress tests using multiple threads to input data into the Data Feed.  The proxy and service handled sustaianed (5 to 10 minutes to input data) throughput of over 350 calls per second via 250 threads, each thread doing 1000 calls to the Data Feed service concurrently.  The service was also capable of scaling out so that several instances running concurrently sped up emptying the queue containing over 200,000 items.  All of this without failure.

