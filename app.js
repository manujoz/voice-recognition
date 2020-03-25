const { VoiceRecognizer } = require("./src/voice-recognition");

const recognizer = new VoiceRecognizer();

module.exports.recognizer = recognizer;