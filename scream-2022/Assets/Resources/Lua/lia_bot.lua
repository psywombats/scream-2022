if getSwitch('n2_clear') then
    speak("Lia", "L-let's go visiting.")
elseif getSwitch('day1_08_meeting') then
    speak("Lia", "Where should we look next?", 'lia_bot')
else
    speak("Tess", "Let's go visiting. There are three people I'd like you to meet.")
end
