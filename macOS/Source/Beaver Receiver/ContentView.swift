import SwiftUI

struct ContentView: View {
    @EnvironmentObject var appDelegate: AppDelegate
    @State private var customURL: String = ""
    @State private var showInvalidURLAlert: Bool = false
    @State private var selectedStream: String?

    let streams = [
        "Beat Blender": "https://ice4.somafm.com/beatblender-128-aac",
        "Black Rock": "https://ice4.somafm.com/brfm-128-aac",
        "Boot Liquor": "https://ice4.somafm.com/bootliquor-128-aac",
        "Bossa Beyond": "https://ice4.somafm.com/bossa-128-aac",
        "Chillits": "https://ice4.somafm.com/chillits-128-aac",
        "Cliqhop": "https://ice4.somafm.com/cliqhop-128-aac",
        "Covers": "https://ice4.somafm.com/covers-128-aac",
        "Deep Space One": "https://ice4.somafm.com/deepspaceone-128-aac",
        "DEF CON Radio": "https://ice4.somafm.com/defcon-128-aac",
        "Digitalis": "https://ice4.somafm.com/digitalis-128-aac",
        "Doomed": "https://ice4.somafm.com/doomed-128-aac",
        "Drone Zone": "https://ice4.somafm.com/dronezone-128-aac",
        "Dub Step Beyond": "https://ice4.somafm.com/dubstep-128-aac",
        "Fluid": "https://ice4.somafm.com/fluid-128-aac",
        "Folk Forward": "https://ice4.somafm.com/folkfwd-128-aac",
        "Groove Salad": "https://ice4.somafm.com/groovesalad-128-aac",
        "Groove Salad Classic": "https://ice4.somafm.com/gsclassic-128-aac",
        "Heavyweight Reggae": "https://ice4.somafm.com/reggae-128-aac",
        "Illinois Street Lounge": "https://ice4.somafm.com/illstreet-128-aac",
        "Indie Pop Rocks!": "https://ice4.somafm.com/indiepop-128-aac",
        "Left Coast 70s": "https://ice4.somafm.com/seventies-128-aac",
        "Lush": "https://ice4.somafm.com/lush-128-aac",
        "Metal Detector": "https://ice4.somafm.com/metal-128-aac",
        "Mission Critical": "https://ice4.somafm.com/missioncontrol-128-aac",
        "n5MD": "https://ice4.somafm.com/n5md-128-aac",
        "PopTron": "https://ice4.somafm.com/poptron-128-aac",
        "Secret Agent": "https://ice4.somafm.com/secretagent-128-aac",
        "Seven Inch Soul": "https://ice4.somafm.com/7soul-128-aac",
        "SF 10-33": "https://ice4.somafm.com/sf1033-128-aac",
        "SF in SF": "https://ice4.somafm.com/sfinsf-128-aac",
        "SF Police Scanner": "https://ice4.somafm.com/scanner-128-aac",
        "SomaFM Live": "https://ice4.somafm.com/live-128-aac",
        "SomaFM Specials": "https://ice4.somafm.com/specials-128-aac",
        "Sonic Universe": "https://ice4.somafm.com/sonicuniverse-128-aac",
        "Space Station Soma": "https://ice4.somafm.com/spacestation-128-aac",
        "Suburbs of Goa": "https://ice4.somafm.com/suburbsofgoa-128-aac",
        "Synphaera Radio": "https://ice4.somafm.com/synphaera-128-aac",
        "The Dark Zone": "https://ice4.somafm.com/darkzone-128-aac",
        "The In-Sound": "https://ice4.somafm.com/insound-128-aac",
        "The Trip": "https://ice4.somafm.com/thetrip-128-aac",
        "ThistleRadio": "https://ice4.somafm.com/thistle-128-aac",
        "Tiki Time": "https://ice4.somafm.com/tikitime-128-aac",
        "Underground 80s": "https://ice4.somafm.com/u80s-128-aac",
        "Vaporwaves": "https://ice4.somafm.com/vaporwaves-128-aac"
    ]

    var body: some View {
        VStack(spacing: 20) {
            // Title and description
            Text("Beaver Receiver")
                .font(.largeTitle)
                .fontWeight(.bold)
                .foregroundColor(.brown)
                .multilineTextAlignment(.center)

            Text("manage a stream from the menu bar")
                .font(.title2)
                .foregroundColor(.gray)
                .multilineTextAlignment(.center)

            Divider()
                .padding(.horizontal)

            // Stream picker and Apply button
            HStack(spacing: 16) {
                Picker("Choose one of these streams", selection: $selectedStream) {
                    ForEach(streams.keys.sorted(), id: \.self) { stream in
                        Text(stream).tag(stream)
                    }
                }
                .pickerStyle(MenuPickerStyle())
                .onChange(of: selectedStream) { newValue in
                    if let streamKey = newValue, let streamURLString = streams[streamKey], let streamURL = URL(string: streamURLString) {
                        appDelegate.setStreamURL(url: streamURL)
                    }
                }
                .padding(.horizontal)

                Button("Apply") {
                    if let selectedStream = selectedStream, let streamURLString = streams[selectedStream], let streamURL = URL(string: streamURLString) {
                        appDelegate.setStreamURL(url: streamURL)
                        appDelegate.togglePlayback() // Start playback when "Apply" is clicked
                    }
                }
                .buttonStyle(.borderedProminent)
                .padding(.trailing)
            }

            // Custom URL entry
            VStack(alignment: .leading, spacing: 16) {
                Text("or swim in your own")
                    .font(.headline)
                    .foregroundColor(.gray)

                HStack {
                    TextField("Enter custom stream URL", text: $customURL)
                        .textFieldStyle(RoundedBorderTextFieldStyle())
                        .onSubmit {
                            applyCustomURL()
                        }

                    Button("Apply") {
                        applyCustomURL()
                    }
                    .buttonStyle(.borderedProminent)
                    .padding(.leading, 8)
                }
            }
            .padding(.horizontal)

            Spacer()
            VStack {
                Text("Volume")
                Slider(value: $appDelegate.volume, in: 0.0...1.0, step: 0.01) { _ in
                    appDelegate.setVolume(level: appDelegate.volume)
                }
            }
            .padding(.horizontal)

            Spacer()

            // Current status and stream URL
            VStack {
                Text("Currently Playing: \(appDelegate.currentStreamURL.absoluteString)")
                    .font(.footnote)
                    .multilineTextAlignment(.center)
                    .padding(.horizontal)

                Text("Playback Status: \(appDelegate.playbackStatus)")
                    .font(.footnote)
                    .foregroundColor(appDelegate.playbackStatus == "Playing" ? .green : .red)
            }
            .padding(.bottom, 20)
            
        }
        .padding()
        .alert(isPresented: $showInvalidURLAlert) {
            Alert(
                title: Text("Invalid URL"),
                message: Text("The URL you entered is invalid. Please check it or try opening it in a web browser."),
                dismissButton: .default(Text("OK"))
            )
        }
    }

    // Apply button logic for custom URL
    private func applyCustomURL() {
        if let url = URL(string: customURL), customURL.lowercased().hasPrefix("http://") || customURL.lowercased().hasPrefix("https://") {
            appDelegate.setStreamURL(url: url)
        } else {
            showInvalidURLAlert = true
        }
    }
}

