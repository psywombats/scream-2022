if not getSwitch('pt1_09a') or not getSwitch('pt1_09b') or not getSwitch('pt1_09c') or not getSwitch('pt1_09d') then
	return
end

wait(1)
setSwitch('glitch_on', true)
wait(0.5)
setSwitch('no_settings', true)
setSwitch('pt1_10', true)
teleport('gazer', 'chair', 'SOUTH', true)
wait(0.5)
setSwitch('no_settings', false)
setSwitch('glitch_on', false)
wait(1)

setting("March 1st, 8:00PM")
wait(0.7)
speak("ARIEL", "It's already time to head home?")
speak("ARIEL", "Noemi will probably still be around. I wonder if anyone else is too.")

setSwitch('pt1_10', true)
