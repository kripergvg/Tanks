## Структура
Игровая сцена разделена на несколько сцен, чтобы разным людям можно было одновременно редактировать разные сцены
 - Startup - основаная сцена 
 - CoreEnvironment - сцена с лвл дизайном
 - PlayerUI - интерфейс игрока

Настройки танка хранятся в его префабе Entities/Tank/Tank
Передвижение танка сделано за счет RigidBody (учтена интерполяция)
Логика способностей находится в AbilitySystem
Настройки способностей хранятся в ScriptableObject: (Entities\Tank\Abilities\Meta\Missile\SimpleMissile.asset), (Entities\Tank\Abilities\Meta\MachineGun\Simple Machine Gun Ability.asset)
В текущей реализации два вида мобов 
- BigZombie (Entities/Mobs/BigZombie) и SimpleZombie (Entities/Mobs/SimpleZombie)


## Что не успел
- Написать тесты
- Некоторые классы не успел отделить от юнити
- Сделать интерфейс старта и окончания игры
- Различные визуальные эффекты
- Доработать ссылки между сценами
