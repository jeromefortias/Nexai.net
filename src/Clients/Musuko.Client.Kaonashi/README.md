# Ollama Chatbot - Windows Desktop Application

A modern C# .NET Core Windows Forms application with a beautiful dark-themed UI for interacting with Ollama AI models.

## Features

- 🤖 **Chat with Ollama Models**: Interactive chatbot interface with real-time streaming responses
- ⚙️ **Configurable Connection**: Manually set the Ollama server host and port
- 🎯 **Model Selection**: Choose from all available Ollama models on your system
- 💾 **Chat History**: Maintains conversation context throughout the session
- 🎨 **Modern Dark UI**: Beautiful Discord-inspired dark theme with smooth animations
- 📝 **Persistent Settings**: Automatically saves your connection preferences
- 🔴 **Connection Status**: Visual indicator showing real-time connection status
- ⌨️ **Keyboard Shortcuts**: Ctrl+Enter to send messages quickly
- 🎭 **Custom System Prompts**: Define the AI's personality and behavior
- 🧠 **Think Tag Filtering**: Hide `<think>` tags to show only final responses
- 🎭 **Dynamic Marvin Expressions**: Character eyes change based on conversation sentiment

## UI Design

The application features a modern dark theme inspired by popular communication apps:

### Main Window
- **Dark Background**: Easy on the eyes with a sophisticated color palette
- **Header Panel**: Model selector, settings, and clear buttons with status indicator
- **Chat Area**: Large, readable text with color-coded messages
- **Input Panel**: Spacious multi-line input with prominent Send button
- **Status Footer**: Quick tips and connection information

### Color Scheme
- **Primary Color**: Discord-inspired purple (#5865F2) for interactive elements
- **Success Color**: Green (#43B581) for connected status
- **Warning Color**: Orange (#FAA61A) for attention items
- **Error Color**: Red (#ED4245) for errors
- **Background**: Dark gray tones for comfortable extended use

### Settings Dialog
- Matching dark theme for consistency
- Clear, organized layout
- Real-time validation
- Smooth hover effects on buttons

## Prerequisites

- **.NET 8.0 SDK** or later
- **Ollama** installed and running on your system
  - Download from: https://ollama.ai
  - At least one model pulled (e.g., `ollama pull llama2`)

## Building the Project

### Using Visual Studio 2022
1. Open the `OllamaChatbot` folder in Visual Studio
2. Build the solution (Ctrl+Shift+B)
3. Run the application (F5)

### Using .NET CLI
```bash
cd OllamaChatbot
dotnet restore
dotnet build
dotnet run
```

### Creating an Executable
```bash
cd OllamaChatbot
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

The executable will be in: `bin/Release/net8.0-windows/win-x64/publish/OllamaChatbot.exe`

## Usage

### First Launch
1. Launch the application
2. If Ollama is running on the default port (11434), it will automatically connect
3. Select a model from the dropdown menu
4. Start chatting!

### Changing Connection Settings
1. Click the **⚙ Settings** button
2. Enter your Ollama server details:
   - **Host**: Server address (default: `localhost`)
   - **Port**: Server port (default: `11434`)
   - **Default Model**: Preferred model name
   - **System Prompt**: Define the AI's behavior and personality
   - **Hide <think> tags**: Filter out AI reasoning tokens (checked by default)
3. Click **Save**

The application will reconnect and reload available models.

### System Prompt
The system prompt allows you to customize how the AI assistant behaves. Examples:
- `You are a helpful coding assistant specialized in Python.`
- `You are a creative writing assistant who speaks in a poetic style.`
- `You are a professional business consultant. Keep responses concise.`

The system prompt is included with every message to maintain consistent behavior.

### Think Tag Filtering
Some AI models (like deepseek-r1) output their reasoning process wrapped in `<think>` tags before providing the final answer. The "Hide <think> tags" option (enabled by default) filters out this reasoning content, showing only the final response. 

**Example:**
```
Input: <think>Let me analyze this...</think>The answer is 42.
Output (filtered): The answer is 42.
```

Uncheck this option in settings if you want to see the AI's reasoning process.

### Using the Chat Interface
- Type your message in the dark text box at the bottom
- Click **Send ➤** or press **Ctrl+Enter** to send
- Press **UP arrow** to recall your previous prompts (like terminal history)
- Press **DOWN arrow** to navigate forward through prompt history
- Watch responses stream in real-time with syntax highlighting
- Messages are color-coded: You (purple), Assistant (green), System (gray)
- Click **🗑 Clear** to start a new conversation (also clears prompt history)
- Connection status indicator shows green when connected

## Configuration

Settings are saved in: `%APPDATA%/OllamaChatbot/config.json`

Default configuration:
```json
{
  "OllamaHost": "localhost",
  "OllamaPort": 11434,
  "DefaultModel": "llama2",
  "SystemPrompt": "You are a helpful AI assistant.",
  "HideThinkTags": true
}
```

## Keyboard Shortcuts

- **Ctrl+Enter**: Send message quickly without clicking the Send button
- **UP Arrow**: Navigate to previous messages in your prompt history
- **DOWN Arrow**: Navigate to next messages in your prompt history (or clear input)

## Troubleshooting

### "Cannot connect to Ollama"
- Ensure Ollama is running: `ollama serve`
- Check if the port is correct in Settings
- Verify firewall settings

### "No models found"
- Pull at least one model: `ollama pull llama2`
- Restart the application

### Port Already in Use
- Change the Ollama port in Settings
- Or change Ollama's port: `OLLAMA_HOST=0.0.0.0:11435 ollama serve`

## Project Structure

```
OllamaChatbot/
├── OllamaChatbot.csproj  # Project configuration
├── Program.cs            # Application entry point
├── MainForm.cs           # Main chat interface
├── SettingsForm.cs       # Settings dialog
├── OllamaService.cs      # Ollama API client
├── AppConfig.cs          # Configuration management
└── README.md             # This file
```

## Dependencies

- **.NET 8.0 Windows Forms**
- **Newtonsoft.Json** (13.0.3) - JSON serialization

## License

This project is provided as-is for personal and educational use.

## Contributing

Feel free to fork, modify, and improve this application!

## Support

For Ollama-related questions: https://github.com/ollama/ollama

