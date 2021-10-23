if getSwitch('d4_clear') then
    speak("Out of service.")
elseif getSwitch('spawn_lia') then
    speak('Tess', "This is the elevator. You can't use it without a keycard.")
else
    search("The elevator requires a keycard to use.")
end