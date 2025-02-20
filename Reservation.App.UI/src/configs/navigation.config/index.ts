import { NAV_ITEM_TYPE_ITEM } from '@/constants/navigation.constant'
import type { NavigationTree } from '@/@types/navigation'
import { ADMIN_PREFIX_PATH } from '@/constants/route.constant'

const navigationConfig: NavigationTree[] = [
    {
        key: 'reservations',
        path: `${ADMIN_PREFIX_PATH}/reservations`,
        title: 'Резервации',
        translateKey: '',
        icon: 'reservation',
        type: NAV_ITEM_TYPE_ITEM,
        authority: [],
        subMenu: [],
    },
]

export default navigationConfig
