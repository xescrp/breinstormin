ProfilerOBJ project
===================

This project builds an unmanaged dll that is loaded into the process of the application
being profiled.

It is a COM component that receives profiling notifications from the CLR and responds
to them by writing into a log file.



type
  ICorProfilerCallback = interface;
  PULONGArray = ^TULONGArray;
  TULONGArray = array[0..16387] of ULONG;
  ICorProfilerCallback = interface(IUnknown)
    ['{176FBED1-A55C-4796-98CA-A9DA0EF883E7}']
    // STARTUP/SHUTDOWN EVENTS
    procedure Initialize(const pICorProfilerInfoUnk: IUnknown); reintroduce; safecall;
    procedure Shutdown; safecall;
    // COR_PRF_MONITOR_APPDOMAIN_LOADS
    procedure AppDomainCreationStarted(appDomainId: ULONG); safecall;
    procedure AppDomainCreationFinished(appDomainId: ULONG; hrStatus: HResult); safecall;
    procedure AppDomainShutdownStarted(appDomainId: ULONG); safecall;
    procedure AppDomainShutdownFinished(appDomainId: ULONG; hrStatus: HResult); safecall;
    // COR_PRF_MONITOR_ASSEMBLY_LOADS
    procedure AssemblyLoadStarted(assemblyId: ULONG); safecall;
    procedure AssemblyLoadFinished(assemblyId: ULONG; hrStatus: HResult); safecall;
    procedure AssemblyUnloadStarted(assemblyId: ULONG); safecall;
    procedure AssemblyUnloadFinished(assemblyId: ULONG; hrStatus: HResult); safecall;
    // COR_PRF_MONITOR_MODULE_LOADS
    procedure ModuleLoadStarted(moduleId: ULONG); safecall;
    procedure ModuleLoadFinished(moduleId: ULONG; hrStatus: HResult); safecall;
    procedure ModuleUnloadStarted(moduleId: ULONG); safecall;
    procedure ModuleUnloadFinished(moduleId: ULONG; hrStatus: HResult); safecall;
    procedure ModuleAttachedToAssembly(moduleId: ULONG; assemblyId: ULONG); safecall;
    // COR_PRF_MONITOR_CLASS_LOADS
    procedure ClassLoadStarted(classId: ULONG); safecall;
    procedure ClassLoadFinished(classId: ULONG; hrStatus: HResult); safecall;
    procedure ClassUnloadStarted(classId: ULONG); safecall;
    procedure ClassUnloadFinished(classId: ULONG; hrStatus: HResult); safecall;
    // COR_PRF_MONITOR_FUNCTION_UNLOADS
    procedure FunctionUnloadStarted(functionId: ULONG); safecall; //currently inactive
    // COR_PRF_MONITOR_JIT_COMPILATION
    procedure JITCompilationStarted(functionId: ULONG; fIsSafeToBlock: Integer); safecall;
    procedure JITCompilationFinished(functionId: ULONG; hrStatus: HResult; fIsSafeToBlock: Integer); safecall;
    function JITInlining(callerId: ULONG; calleeId: ULONG): {fShouldInline} Bool; safecall;
    // COR_PRF_MONITOR_FUNCTION_UNLOADS
    procedure JITFunctionPitched(functionId: ULONG); safecall;
    // COR_PRF_MONITOR_CACHE_SEARCHES
    function JITCachedFunctionSearchStarted(functionId: ULONG): {bUseCachedFunction} Bool; safecall;
    procedure JITCachedFunctionSearchFinished(functionId: ULONG; AResult: COR_PRF_JIT_CACHE); safecall;
    // COR_PRF_MONITOR_THREADS
    procedure ThreadCreated(threadId: ULONG); safecall;
    procedure ThreadDestroyed(threadId: ULONG); safecall;
    procedure ThreadAssignedToOSThread(managedThreadId: ULONG; osThreadId: ULONG); safecall;
    // COR_PRF_MONITOR_REMOTING
    procedure RemotingClientInvocationStarted; safecall;
    procedure RemotingClientSendingMessage(const pCookie: TGuid; fIsAsync: Bool); safecall;
    procedure RemotingClientReceivingReply(const pCookie: TGuid; fIsAsync: Bool); safecall;
    procedure RemotingClientInvocationFinished; safecall;
    procedure RemotingServerReceivingMessage(const pCookie: TGuid; fIsAsync: Bool); safecall;
    procedure RemotingServerInvocationStarted; safecall;
    procedure RemotingServerInvocationReturned; safecall;
    procedure RemotingServerSendingReply(const pCookie: TGuid; fIsAsync: Bool); safecall;
    // COR_PRF_MONITOR_CODE_TRANSITIONS
    procedure UnmanagedToManagedTransition(functionId: ULONG; reason: COR_PRF_TRANSITION_REASON); safecall;
    procedure ManagedToUnmanagedTransition(functionId: ULONG; reason: COR_PRF_TRANSITION_REASON); safecall;
    // COR_PRF_MONITOR_SUSPENDS
    procedure RuntimeSuspendStarted(suspendReason: COR_PRF_SUSPEND_REASON); safecall;
    procedure RuntimeSuspendFinished; safecall;
    procedure RuntimeSuspendAborted; safecall;
    procedure RuntimeResumeStarted; safecall;
    procedure RuntimeResumeFinished; safecall;
    procedure RuntimeThreadSuspended(threadId: ULONG); safecall;
    procedure RuntimeThreadResumed(threadId: ULONG); safecall;
    // COR_PRF_MONITOR_GC
    procedure MovedReferences(cMovedObjectIDRanges: ULONG; var oldObjectIDRangeStart, newObjectIDRangeStart, cObjectIDRangeLength: TULONGArray); safecall;
    procedure ObjectsAllocatedByClass(cClassCount: ULONG; var classIds, cObjects: TULONGarray); safecall;
    procedure ObjectReferences(objectId: ULONG; classId: ULONG; cObjectRefs: ULONG; var objectRefIds: TULONGArray); safecall;
    procedure RootReferences(cRootRefs: ULONG; var rootRefIds: TULONGArray); safecall;
    // COR_PRF_MONITOR_OBJECT_ALLOCATED
    procedure ObjectAllocated(objectId, classId: ULONG); safecall;
    // COR_PRF_MONITOR_EXCEPTIONS
    procedure ExceptionThrown(thrownObjectId: ULONG); safecall;
    procedure ExceptionSearchFunctionEnter(functionId: ULONG); safecall;
    procedure ExceptionSearchFunctionLeave; safecall;
    procedure ExceptionSearchFilterEnter(functionId: ULONG); safecall;
    procedure ExceptionSearchFilterLeave; safecall;
    procedure ExceptionSearchCatcherFound(functionId: ULONG); safecall;
    procedure ExceptionOSHandlerEnter(__unused: ULONG); safecall; //currently inactive
    procedure ExceptionOSHandlerLeave(__unused: ULONG); safecall; //currently inactive
    procedure ExceptionUnwindFunctionEnter(functionId: ULONG); safecall;
    procedure ExceptionUnwindFunctionLeave; safecall;
    procedure ExceptionUnwindFinallyEnter(functionId: ULONG); safecall;
    procedure ExceptionUnwindFinallyLeave; safecall;
    procedure ExceptionCatcherEnter(functionId: ULONG; objectId: ULONG); safecall;
    procedure ExceptionCatcherLeave; safecall;
    // COR_PRF_MONITOR_CLR_EXCEPTIONS
    procedure ExceptionCLRCatcherFound; safecall;
    procedure ExceptionCLRCatcherExecute; safecall;
    // COR_PRF_MONITOR_CCW
    procedure COMClassicVTableCreated(wrappedClassId: ULONG; const implementedIID: TGuid; pVTable: Pointer; cSlots: ULONG); safecall;
    procedure COMClassicVTableDestroyed(wrappedClassId: ULONG; const implementedIID: TGuid; pVTable: Pointer); safecall;
  end;