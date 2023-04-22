if getSwitch('swap_floor') then
	speak('ARIEL', "I'm on the 36th floor, so I'll head up to the 37th.")
else
	speak('ARIEL', "I'm the the 37th floor, so I'll head down to the 36th.")
end
-- TODO: plock
wait(.9)
-- TODO: hum
elevate()
wait(4)
setSwitch('swap_floor', not getSwitch('swap_floor'))
