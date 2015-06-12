# Blog2-WcfNQueueSMEx2
An SO App is a Service Oriented App.

This is the CODE for the blog article "SO Apps 2, WcfNQueueSMEx2 – A System of Collaborating Microservices" at  https://dotnetsilverlightprism.wordpress.com/2015/06/12/so-apps-2-wcfnqueuesmex2-a-system-of-collaborating-microservices/
*
*  The Setup Instructions document says how to install ServiceModelEx and setup Azure, and is up-to-date.
*  
ITERATION 2 PLAN
1. DONE -- Design and implement a Visual Studio solution structure ammenable to service oriented apps -- A very flat structure so that the resueable components can easily be seen by scrolling, rather than expanding a deep hierarchy of folders that serves to hide components.
2. DONE -- Design and implement the Data Feed subsystem per the IDesign Method, using Engines and ResourceAccessors to do the work, leaving only the orchestration of the work to the Manager code.  DONE -- Decoupling ResourceAccessor from specific persistence mechanisms via repositories and their interfaces.
3. DONE -- Develop system diagrams showing software structure.  
4. DONE -- Add a 3rd Manager to further demonstrate vertical slices, reuse of components, and microservices.  
5. NOTE -- 5-28-15  This code passed a series of rigorous stress tests using multiple threads to input data into the Data Feed.  The proxy and service handled sustained (5 to 10 minutes to input data) throughput of over 350 calls per second via 250 threads, each thread doing 1000 calls to the Data Feed service concurrently.  The service was also capable of scaling out so that up to 5 service instances running concurrently sped up emptying the queue containing over 200,000 items.  All of this without failure.
7. NOTE -- 6-12-15  Released the code, supporting documents, and blog article on WordPress.

