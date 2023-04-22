if not getSwitch("announced_f3") then
	setSwitch('midnight', true)
	setSwitch("announced_f3", true)
	--wait(.4)
	--setting("March 2nd 12:00AM")
	--setting("Aquila Tower Floor 36")
elseif not getSwitch('midnight') then
	setting("Aquila Tower")
	setting("Floor 37")
end
