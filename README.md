# Blog2-WcfNQueueSMEx2
An SO App is a Service Oriented App.

Code for the blog article "SO Apps 2, WcfNQueueSMEx2 – A System of Collaborating Microservices" at  https://dotnetsilverlightprism.wordpress.com/2015/05/12/TobeDetermined/

*  This Iteration 2 project is CURRENTLY BEING RELEASED and WILL WORK!!  The blog article is being released.

*  The Setup Instructions document says how to install ServiceModelEx and setup Azure, and is up-to-date.
*  
ITERATION 2 PLAN
1. DONE -- Design and implement a Visual Studio solution structure ammenable to service oriented apps -- A very flat structure so that the resueable components can easily be seen by scrolling, rather than expanding a deep hierarchy of folders that serves to hide components.
2. DONE -- Design and implement the Data Feed subsystem per the IDesign Method, using Engines and ResourceAccessors to do the work, leaving only the orchestration of the work to the Manager code.  DONE -- Decoupling ResourceAccessor from specific persistence mechanisms via repositories and their interfaces.
3. DONE -- Develop and present system diagrams showing software structure.  
4. DONE -- Add a 3rd Manager to further demonstrate vertical slices, reuse of components, and microservices.  
5. NOTE -- 5-28-15  This code passed a series of rigorous stress tests using multiple threads to input data into the Data Feed.  The proxy and service handled sustained (5 to 10 minutes to input data) throughput of over 350 calls per second via 250 threads, each thread doing 1000 calls to the Data Feed service concurrently.  The service was also capable of scaling out so that up to 5 service instances running concurrently sped up emptying the queue containing over 200,000 items.  All of this without failure.
7. NOTE -- 6-2-15  The DataAccessors and their repositories that decouple them from specific storage and done and organized in their final structure.  Also added the IFeedAdmin contract to the DataFeedManager to demo multiple contracts per manager.  Solution structure is approaching it final state, but still needs a lot of renaming.  System diagrams are near being done.
8. NOTE -- 6-12-15  In the process of releasing stable code with up-to-date documents, and moving the written blog 2 article from Word into Word Press.

