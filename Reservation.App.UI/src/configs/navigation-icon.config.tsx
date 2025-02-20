import { FiBook } from 'react-icons/fi'

export type NavigationIcons = Record<string, JSX.Element>

const navigationIcon: NavigationIcons = {
    reservation: <FiBook />,
}

export default navigationIcon
