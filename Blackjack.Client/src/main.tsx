import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { BrowserRouter } from 'react-router-dom'
import { CookiesProvider } from 'react-cookie'
import { Provider } from 'react-redux'
import { store } from './store/Store.ts'

createRoot(document.getElementById('root')!).render(
	<div>
		<BrowserRouter>
			<Provider store={store}>
				<CookiesProvider>
					<App />
				</CookiesProvider>
			</Provider>
		</BrowserRouter>
	</div>
)
