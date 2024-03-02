﻿using KludgeBox.Events;

namespace KludgeBox.Events.Global;

public readonly record struct ProfileButtonReadyEvent(ProfileButton ProfileButton) : IEvent;
public readonly record struct ProfileButtonClickEvent(ProfileButton ProfileButton) : IEvent;