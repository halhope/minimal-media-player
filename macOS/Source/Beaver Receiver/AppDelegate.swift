import Cocoa
import AVKit

class AppDelegate: NSObject, NSApplicationDelegate, ObservableObject {
    var statusItem: NSStatusItem!
    var player: AVPlayer?
    @Published var currentStreamURL: URL {
        didSet {
            // Save the URL to UserDefaults whenever it changes
            UserDefaults.standard.set(currentStreamURL.absoluteString, forKey: "lastStreamURL")
        }
    }
    @Published var playbackStatus: String = "Stopped"
    @Published var volume: Float
    override init() {
        // Load the saved URL from UserDefaults or default to the Groove Salad stream
        if let savedURLString = UserDefaults.standard.string(forKey: "lastStreamURL"),
           let savedURL = URL(string: savedURLString) {
            self.currentStreamURL = savedURL
        } else {
            self.currentStreamURL = URL(string: "https://ice4.somafm.com/groovesalad-128-aac")!
        }
        let savedVolume = UserDefaults.standard.float(forKey: "playerVolume")
        let initialVolume = savedVolume > 0.0 ? savedVolume : 1.0 // Ensure a valid default
        
        self.volume = initialVolume
        super.init()
    }
    
    func applicationDidFinishLaunching(_ notification: Notification) {
        setupMenuBar()
        playbackStatus = "Stopped"  // Ensure the app starts with music stopped
    }
    
    private func setupMenuBar() {
        // Create the status item for the menu bar
        statusItem = NSStatusBar.system.statusItem(withLength: NSStatusItem.variableLength)
        
        // Set the icon for the menu bar button
        if let button = statusItem.button {
            button.title = "🪵" // Initial icon for "Play"
            button.action = #selector(togglePlayback)
            button.target = self
        }
    }
    
    // Change access level to internal or public
    @objc func togglePlayback() {
        if let player = player, player.timeControlStatus == .playing {
            player.pause()
            playbackStatus = "Stopped"
            statusItem.button?.title = "🪵" // Icon for "Play"
        } else {
            // Initialize player only when playback is toggled
            let playerItem = AVPlayerItem(url: currentStreamURL)
            player = AVPlayer(playerItem: playerItem)
            player?.play()
            playbackStatus = "Playing"
            statusItem.button?.title = "💧" // Icon for "Stop"
        }
    }
    
    func setStreamURL(url: URL) {
        currentStreamURL = url
        playbackStatus = "Stopped" // Ensure it starts as stopped
        
        // Ensure the player is paused if it's playing
        player?.pause()
        
        // Create a new player item with the selected stream URL, but don't start playback
        let playerItem = AVPlayerItem(url: url)
        player = AVPlayer(playerItem: playerItem)
        player?.volume = volume // Ensure volume is applied to new stream
    }
    func setVolume(level: Float) {
        volume = level
        player?.volume = level
        UserDefaults.standard.set(level, forKey: "playerVolume")
    }
}

