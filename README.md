# Akyuu

An automated meeting screen recording/transcribing service between OBS, OpenAI's Whisper, and GPT.
Well, at least eventually, this project is going to take a while.

Because I have the memory of a goldfish.

## Status
probably on fire ¯\\\_(ツ)\_/¯

task list of things that could go wrong
- [x] Meeting detector actually detecting (UDP sniff)
  - works on Discord (US West voice servers) and Teams (IDK I pulled some IPs off Microsoft's website)
- [x] """mini""" OBS WebSocket library
  - implemented most of the opcodes, even though I really just needed start/stop recording
- [ ] OpenAI connections
  - [ ] Whisper (or some other transcribing library)
    - Probably call the Python executable in the venv
  - [ ] some GPT depending on if I get GPT-4 access, otherwise ChatGPT
    - fun fun API access
- [ ] small API server to unite all components
- [ ] (extra credit) demo GUI
  - please don't be WPF (probably will be, unless the API server gets a webpage)