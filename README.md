# BetterSCP939
An **upgraded** version of the already existing **SCP-939**.

**SCP-939** size has been **reduced by 25%**, it's **faster than humans** and will get **slowed down** after hurting someone.
In return, its **attack** will be **boosted** based on the damage taken.
The adrenaline bar will show how much **angry** it is.

**Credits to Reddking#2021 for the great idea.**

## Minimum requirements
[EXILED](https://github.com/galaxy119/EXILED) **1.9.10+**

## How to install
Put **BetterSCP939.dll** inside `%appdata%\Plugins` if you're on **Windows** or `~/.config/Plugins` on **Linux**.

### Configs
| Name | Type | Default Value | Description |
| --- | :---: | :---: | --- |
| b939_enabled | Boolean | True | Enable/Disable the plugin. |
| b939_size | Float | 0.75 | The size of SCP-939. |
| b939_slow_amount | Float | 10 | How much SCP-939 will be slowed down after hurting someone (higher is faster). |
| b939_force_slow_down_time | Float | 3 | For how many seconds SCP-939 will be slowed down after hurting someone. |
| b939_base_damage | Float | 40 | The base damage that SCP-939 will inflict. |
| b939_bonus_attack_maximum | Float | 150 | The maximum amount of bonus attack that SCP-939 can inflict. |
| b939_anger_meter_maximum | Float | 500 | The maximum amount of SCP-939 anger. |
| b939_anger_meter_decay_time | Float | 1 | After how many seconds, the anger meter will start to decay. |
| b939_anger_meter_decay_value| Float | 3 | How much the anger meter will decay. |
| b939_starting_anger | Float | 0 | The starting value of anger of SCP-939. |
| b939_show_spawn_broadcast_message | Boolean | False | If enabled, a broadcast message will be shown to SCP-939 after its spawn. |
| b939_spawn_broadcast_message_duration | Unsigned Integer | 15 | The duration of the SCP-939 spawn broadcast message. |
| b939_spawn_broadcast_message | String | <size=20><color=#00FFFF>You've spawned as an upgraded version of <color=#FF0000>SCP-939</color>!\nYou're faster than humans, your <color=#FF0000>anger</color> will increase after taking damage from them.\nMore anger means more damage inflicted to humans.\nAfter <color=#FF0000>hurting</color> someone, you'll get slowed down for <color=#FF0000>{0}</color> seconds</color></size> | The broadcast message that will be shown to SCP-939 after its spawn. |
