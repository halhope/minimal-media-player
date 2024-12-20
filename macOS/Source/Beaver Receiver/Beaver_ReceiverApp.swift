import SwiftUI

@main
struct Beaver_ReceiverApp: App {
    // Use NSApplicationDelegateAdaptor for macOS apps
    @NSApplicationDelegateAdaptor(AppDelegate.self) var appDelegate

    var body: some Scene {
        WindowGroup {
            ContentView()
                .environmentObject(appDelegate) // Make sure AppDelegate is passed as an environment object
        }
    }
}
